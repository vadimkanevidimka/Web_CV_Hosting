// src/context/AuthContext.tsx (ПОЛНОСТЬЮ ИСПРАВЛЕННЫЙ)
import { createContext, useState, useEffect, useContext } from 'react';
import type { ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from '../axiosConfig'; // 'allowJs: true' в tsconfig.json исправит ошибку импорта

// 1. Описываем тип пользователя (допустим, он такой)
interface User {
  email: string;
  // Добавь другие поля, если они есть
}

// 2. Описываем тип, который будет храниться в Контексте
interface AuthContextType {
  user: User | null;
  token: string | null;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, userName: string, password: string, confirmPassword: string) => Promise<boolean>
  logout: (shouldNavigate?: boolean) => void;
}

// 3. Создаем контекст с правильным типом
const AuthContext = createContext<AuthContextType | null>(null);

// 4. Типизируем props для AuthProvider
interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  // 5. Типизируем state
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState(localStorage.getItem('key') || null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const validateToken = async () => {
      if (token) {
        try {
          // Проверяем валидность токена
          const { data } = await axios.get('api/Auth/me');
          setUser(data);
        } catch (error) {
          // Если токен невалидный, очищаем
          localStorage.removeItem('key');
          setToken(null);
          setUser(null);
        } finally {
          setLoading(false);
        }
      } else {
        setLoading(false);
      }
    };

    validateToken();

    // Слушаем событие unauthorized
    const handleUnauthorized = () => {
      logout(true);
    };

    window.addEventListener('unauthorized', handleUnauthorized);
    return () => {
      window.removeEventListener('unauthorized', handleUnauthorized);
    };
  }, [token]);

  // 6. Типизируем параметры функции
  const login = async (email: string, password: string) => {
    try {
      // Логин - сервер сам установит cookies
      await axios.post('api/Auth/login', { email, password });
      
      console.log('✅ Login successful! Server set cookies automatically');
      
      // Проверяем, что cookie установлен
      const cookieValue = document.cookie.split('; ').find(row => row.startsWith('key='));
      if (cookieValue) {
        const token = cookieValue.split('=')[1];
        console.log('✅ Token found in cookie:', token.substring(0, 20) + '...');
        setToken(token);
        localStorage.setItem('key', token); // Дублируем для совместимости
      }
      
      setUser({ email }); // Устанавливаем пользователя
      navigate('/profiles');
    } catch (error: any) { // 7. Типизируем ошибку
      console.error('Ошибка при входе:', error.response?.data?.message || error.message);
      throw error; 
    }
  };

  const register = async (email: string, userName: string, password: string, confirmPassword: string) => {
    try {
      const { data } = await axios.post("api/Auth/register", {email, userName, password, confirmPassword});
      if(data != null){
        return true;
      }
      return false;
    } catch (error: any) {
      console.error('Ошибка при входе:', error.response?.data?.message || error.message);
      throw error; 
    }
  };

  const logout = async (shouldNavigate = true) => {
    try {
      // Вызываем logout на сервере, чтобы он удалил cookie
      await axios.post('api/Auth/logout');
    } catch (error) {
      console.error('Ошибка при logout на сервере:', error);
    }
    
    setUser(null);
    setToken(null);
    localStorage.removeItem('key');
    localStorage.removeItem('refreshkey');
    delete axios.defaults.headers.common['Authorization'];
    
    if (shouldNavigate) {
      navigate('/login');
    }
  };

  // 8. Создаем объект, который соответствует типу AuthContextType
  const contextValue: AuthContextType = {
    user,
    token,
    loading,
    login,
    register,
    logout,
  };

  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
};

// 9. Создаем кастомный хук - это лучшая практика
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export default AuthContext;