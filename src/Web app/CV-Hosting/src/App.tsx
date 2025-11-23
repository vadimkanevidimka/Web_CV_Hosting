// src/App.tsx
import { Routes, Route, useNavigate, useLocation } from 'react-router-dom';
import { 
  AppBar, 
  Toolbar, 
  Typography, 
  Button, 
  Box, 
  IconButton,
  Menu,
  MenuItem,
  Avatar,
  Container
} from '@mui/material';
import { 
  AccountCircle as AccountCircleIcon,
  Description as DescriptionIcon,
  ExitToApp as ExitToAppIcon
} from '@mui/icons-material';
import { useState } from 'react';
import { AuthProvider, useAuth } from './context/AuthContext';
import Register from './pages/Register';
import Login from './pages/Login';
import Profile from './pages/Profile';
import CreateProfile from './pages/CreateProfile';

const AppContent = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { user, logout } = useAuth();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    logout();
    handleMenuClose();
    navigate('/login');
  };

  const handleProfile = () => {
    handleMenuClose();
    navigate('/profiles');
  };

  // Скрываем AppBar на страницах логина и регистрации
  const isAuthPage = location.pathname === '/login' || location.pathname === '/register';

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      {!isAuthPage && (
        <AppBar position="static" elevation={2}>
          <Container maxWidth="xl">
            <Toolbar disableGutters>
              {/* Логотип/Название */}
              <DescriptionIcon sx={{ mr: 1 }} />
              <Typography
                variant="h6"
                component="div"
                sx={{ 
                  flexGrow: 0, 
                  fontWeight: 600,
                  cursor: 'pointer',
                  mr: 4
                }}
                onClick={() => navigate('/profiles')}
              >
                CV Management
              </Typography>

              {/* Навигация */}
              <Box sx={{ flexGrow: 1, display: 'flex', gap: 2 }}>
                {user && (
                  <>
                    <Button 
                      color="inherit"
                      onClick={() => navigate('/profiles')}
                      sx={{ 
                        fontWeight: location.pathname === '/profiles' ? 600 : 400,
                        borderBottom: location.pathname === '/profiles' ? '2px solid white' : 'none'
                      }}
                    >
                      Мой профиль
                    </Button>
                    <Button 
                      color="inherit"
                      onClick={() => navigate('/create-profile')}
                      sx={{ 
                        fontWeight: location.pathname === '/create-profile' ? 600 : 400,
                        borderBottom: location.pathname === '/create-profile' ? '2px solid white' : 'none'
                      }}
                    >
                      Создать профиль
                    </Button>
                    <Button 
                      color="inherit"
                      sx={{ fontWeight: 400 }}
                    >
                      Мои CV
                    </Button>
                  </>
                )}
              </Box>

              {/* Пользователь */}
              {user ? (
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                  <Typography variant="body2" sx={{ mr: 1, display: { xs: 'none', sm: 'block' } }}>
                    {user.email}
                  </Typography>
                  <IconButton
                    size="large"
                    onClick={handleMenuOpen}
                    color="inherit"
                  >
                    <Avatar sx={{ width: 32, height: 32 }}>
                      {user.email?.[0].toUpperCase()}
                    </Avatar>
                  </IconButton>
                  <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={handleMenuClose}
                    anchorOrigin={{
                      vertical: 'bottom',
                      horizontal: 'right',
                    }}
                    transformOrigin={{
                      vertical: 'top',
                      horizontal: 'right',
                    }}
                  >
                    <MenuItem onClick={handleProfile}>
                      <AccountCircleIcon sx={{ mr: 1 }} fontSize="small" />
                      Профиль
                    </MenuItem>
                    <MenuItem onClick={handleLogout}>
                      <ExitToAppIcon sx={{ mr: 1 }} fontSize="small" />
                      Выход
                    </MenuItem>
                  </Menu>
                </Box>
              ) : (
                <Box sx={{ display: 'flex', gap: 1 }}>
                  <Button 
                    color="inherit" 
                    onClick={() => navigate('/login')}
                    variant={location.pathname === '/login' ? 'outlined' : 'text'}
                  >
                    Вход
                  </Button>
                  <Button 
                    color="inherit" 
                    onClick={() => navigate('/register')}
                    variant={location.pathname === '/register' ? 'outlined' : 'text'}
                  >
                    Регистрация
                  </Button>
                </Box>
              )}
            </Toolbar>
          </Container>
        </AppBar>
      )}

      {/* Основной контент */}
      <Box component="main" sx={{ flexGrow: 1 }}>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/profiles" element={<Profile />} />
          <Route path="/create-profile" element={<CreateProfile />} />
          <Route path="/" element={<Login />} />
        </Routes>
      </Box>
    </Box>
  );
};

const App = () => {
  return (
    <AuthProvider>
      <AppContent />
    </AuthProvider>
  );
};

export default App;