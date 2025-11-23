// src/pages/Login.tsx (Исправлено)

import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Container, Typography, Box, Snackbar, Alert } from '@mui/material';
import { useAuth } from '../context/AuthContext'; // 👈 ИСПОЛЬЗУЕМ useAuth

const Login = () => {
    // 1. Используем useAuth, чтобы получить типизированный login
    const { login } = useAuth(); 

    const navigate = useNavigate();
    
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null); // Типизировали state ошибки
    const [open, setOpen] = useState(false);
    const [loading, setLoading] = useState(false); // Добавляем loading state

    // 2. Типизируем событие формы
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        setOpen(false);

        try {
            await login(email, password);
        } catch (err: any) { // 3. Типизируем ошибку
            // Используем оператор ?. для безопасного доступа
            setError(err.response?.data?.message || 'Ошибка при выполнении запроса');
            setOpen(true);
        } finally {
            setLoading(false);
        }
    };

    const handleClose = () => {
        setOpen(false);
    };

    return (
        <Container maxWidth="sm">
             <Box 
                component="form" 
                onSubmit={handleSubmit} // 4. Теперь handleSubmit имеет правильный тип
                sx={{ marginTop: 10, display: 'flex', flexDirection: 'column', alignItems: 'center' }}
            >
                <Typography variant="h4" component="h1" gutterBottom>
                    Sign in
                </Typography>
                
                <TextField
                    fullWidth
                    label="Email"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    margin="normal"
                    required
                    disabled={loading}
                />
                <TextField
                    fullWidth
                    label="Пароль"
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    margin="normal"
                    required
                    disabled={loading}
                />
                <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    fullWidth
                    disabled={loading}
                    sx={{ mt: 2 }}
                >
                    Sign in
                </Button>
                <Button
                    type="button"
                    variant="contained"
                    color="primary"
                    disabled={loading}
                    sx={{mt: 4}}
                    onClick={(e) => navigate("/register")}>
                    Not registered yet?
                </Button>
            </Box>

            <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
                <Alert onClose={handleClose} severity="error" sx={{ width: '100%' }}>
                    {error}
                </Alert>
            </Snackbar>
        </Container>
    );
};

export default Login;