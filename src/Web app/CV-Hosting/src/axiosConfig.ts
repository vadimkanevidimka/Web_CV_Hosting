// src/axiosConfig.ts
import axios from 'axios';
import type { AxiosResponse, AxiosError, InternalAxiosRequestConfig } from 'axios';

// Получаем базовый URL из переменных окружения
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL; 

const axiosInstance = axios.create({
    baseURL: API_BASE_URL, 
    timeout: 10000,
    withCredentials: true, // Важно для отправки cookies с запросами
});

let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any, token: string | null = null) => {
    failedQueue.forEach((prom) => {
        if (error) {
            prom.reject(error);
        } else {
            prom.resolve(token);
        }
    });
    failedQueue = [];
};

// Request interceptor - добавляем JWT токен к каждому запросу
axiosInstance.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('key');
        
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
            console.log('🔑 Token attached to request:', token.substring(0, 20) + '...');
            console.log('📤 Authorization header:', config.headers.Authorization);
        } else {
            console.warn('⚠️ No token found in localStorage!');
        }
        
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Response interceptor - обработка ошибок авторизации и refresh token
axiosInstance.interceptors.response.use(
    (response: AxiosResponse) => response,
    async (error: AxiosError) => {
        const originalRequest: any = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
            if (isRefreshing) {
                // Если уже идет обновление токена, добавляем запрос в очередь
                return new Promise((resolve, reject) => {
                    failedQueue.push({ resolve, reject });
                })
                    .then((token) => {
                        originalRequest.headers.Authorization = `Bearer ${token}`;
                        return axiosInstance(originalRequest);
                    })
                    .catch((err) => {
                        return Promise.reject(err);
                    });
            }

            originalRequest._retry = true;
            isRefreshing = true;

            const refreshToken = localStorage.getItem('refreshkey');

            if (!refreshToken) {
                // Нет refresh токена - выходим
                localStorage.removeItem('key');
                localStorage.removeItem('refreshkey');
                window.dispatchEvent(
                    new CustomEvent('unauthorized', { detail: 'Сессия истекла или недействительна.' })
                );
                return Promise.reject(error);
            }

            try {
                // Попытка обновить токен
                const { data } = await axios.post(
                    `${API_BASE_URL}/api/Auth/refresh`,
                    { refreshToken },
                    { withCredentials: true }
                );

                const newToken = data.token || data.accessToken;
                const newRefreshToken = data.refreshToken;

                localStorage.setItem('key', newToken);
                if (newRefreshToken) {
                    localStorage.setItem('refreshkey', newRefreshToken);
                }

                axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;
                originalRequest.headers.Authorization = `Bearer ${newToken}`;

                processQueue(null, newToken);
                isRefreshing = false;

                return axiosInstance(originalRequest);
            } catch (refreshError) {
                // Не удалось обновить токен - выходим
                processQueue(refreshError, null);
                isRefreshing = false;

                localStorage.removeItem('key');
                localStorage.removeItem('refreshkey');
                window.dispatchEvent(
                    new CustomEvent('unauthorized', { detail: 'Сессия истекла или недействительна.' })
                );

                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

export default axiosInstance;
