// src/components/PrivateRoute.tsx (ПОЛНОСТЬЮ ИСПРАВЛЕННЫЙ)
import { Navigate, useLocation } from 'react-router-dom';
import { CircularProgress, Box } from '@mui/material';
import { useAuth } from '../context/AuthContext'; // 👈 Импортируем наш новый хук
import type { ReactNode } from 'react';

interface PrivateRouteProps {
  children: ReactNode;
}

const PrivateRoute = ({ children }: PrivateRouteProps) => {
  // 1. Используем хук useAuth
  const { token, loading } = useAuth();
  const location = useLocation();

  // 2. Пока идет проверка, показываем спиннер
  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  // 3. Если загрузка закончилась и токена нет - редирект
  if (!token) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // 4. Все в порядке, рендерим страницу
  return <>{children}</>;
};

export default PrivateRoute;