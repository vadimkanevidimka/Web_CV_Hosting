import React from 'react';
import ReactDOM from 'react-dom/client';
import { RouterProvider, createBrowserRouter } from 'react-router-dom';

// Импорты из MUI
import { ThemeProvider, createTheme } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

import App from './App';

//import HomePage from './pages/HomePage.jsx';
//import CVDetailPage from './pages/CVDetailPage.jsx';

// Создаем базовую тему MUI (позже ее можно будет кастомизировать)
const theme = createTheme({
  palette: {
    primary: {
      main: '#65C5C7',
    },
    secondary: {
      main: '#89d0d1ff',
    },
  },
});

const router = createBrowserRouter([
  {
    path: '/*',
    element: <App />,
  },
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      {/* CssBaseline сбрасывает стили и применяет фон из темы */}
      <CssBaseline />
      <RouterProvider router={router} />
    </ThemeProvider>
  </React.StrictMode>
);