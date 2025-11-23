// src/pages/Profile.tsx
import { useState, useEffect } from 'react';
import { 
    Container, 
    Box, 
    Typography, 
    Paper, 
    Avatar, 
    TextField, 
    Button, 
    Divider,
    Chip,
    CircularProgress,
    Alert,
    Snackbar,
    Card,
    CardContent
} from '@mui/material';
import { 
    Edit as EditIcon, 
    Save as SaveIcon, 
    Cancel as CancelIcon,
    CloudUpload as CloudUploadIcon 
} from '@mui/icons-material';
import axiosInstance from '../axiosConfig';

// Интерфейс данных пользователя
interface UserProfile {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    avatar?: string;
    bio?: string;
    position?: string;
    company?: string;
    location?: string;
    skills?: string[];
    platforms?: string[]; // Платформы откуда пришел пользователь
}

// Интерфейс CV
interface CV {
    id: string;
    title: string;
    platform: string;
    createdAt: string;
    updatedAt: string;
    isActive: boolean;
}

const Profile = () => {
    const [profile, setProfile] = useState<UserProfile | null>(null);
    const [cvList, setCvList] = useState<CV[]>([]);
    const [loading, setLoading] = useState(true);
    const [editMode, setEditMode] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [openSnackbar, setOpenSnackbar] = useState(false);

    // Временные данные для редактирования
    const [editData, setEditData] = useState<Partial<UserProfile>>({});

    // Загрузка данных профиля
    useEffect(() => {
        fetchProfile();
        fetchUserCVs();
    }, []);

    const fetchProfile = async () => {
        try {
            setLoading(true);
            const response = await axiosInstance.get('/api/user/profile');
            setProfile(response.data);
            setEditData(response.data);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка при загрузке профиля');
            setOpenSnackbar(true);
        } finally {
            setLoading(false);
        }
    };

    const fetchUserCVs = async () => {
        try {
            const response = await axiosInstance.get('/api/cv/user');
            setCvList(Array.isArray(response.data) ? response.data : []);
        } catch (err: any) {
            console.error('Ошибка при загрузке CV:', err);
            setCvList([]);
        }
    };

    const handleEditToggle = () => {
        if (editMode) {
            setEditData(profile || {});
        }
        setEditMode(!editMode);
    };

    const handleSave = async () => {
        try {
            const response = await axiosInstance.put('/api/user/profile', editData);
            setProfile(response.data);
            setEditMode(false);
            setSuccess('Профиль успешно обновлен');
            setOpenSnackbar(true);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка при обновлении профиля');
            setOpenSnackbar(true);
        }
    };

    const handleInputChange = (field: keyof UserProfile) => (
        e: React.ChangeEvent<HTMLInputElement>
    ) => {
        setEditData({ ...editData, [field]: e.target.value });
    };

    const handleCloseSnackbar = () => {
        setOpenSnackbar(false);
        setError(null);
        setSuccess(null);
    };

    if (loading) {
        return (
            <Container maxWidth="lg">
                <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                    <CircularProgress />
                </Box>
            </Container>
        );
    }

    if (!profile) {
        return (
            <Container maxWidth="lg">
                <Box sx={{ mt: 4 }}>
                    <Alert severity="error">Не удалось загрузить данные профиля</Alert>
                </Box>
            </Container>
        );
    }

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            {/* Заголовок профиля */}
            <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
                <Box sx={{ display: 'flex', flexDirection: { xs: 'column', md: 'row' }, gap: 3 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', flexShrink: 0 }}>
                        <Avatar
                            src={profile.avatar}
                            alt={`${profile.firstName} ${profile.lastName}`}
                            sx={{ width: 150, height: 150 }}
                        />
                    </Box>
                    <Box sx={{ flex: 1 }}>
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start' }}>
                            <Box>
                                {editMode ? (
                                    <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
                                        <TextField
                                            label="Имя"
                                            value={editData.firstName || ''}
                                            onChange={handleInputChange('firstName')}
                                            size="small"
                                        />
                                        <TextField
                                            label="Фамилия"
                                            value={editData.lastName || ''}
                                            onChange={handleInputChange('lastName')}
                                            size="small"
                                        />
                                    </Box>
                                ) : (
                                    <Typography variant="h4" gutterBottom>
                                        {profile.firstName} {profile.lastName}
                                    </Typography>
                                )}
                                
                                {editMode ? (
                                    <TextField
                                        fullWidth
                                        label="Должность"
                                        value={editData.position || ''}
                                        onChange={handleInputChange('position')}
                                        size="small"
                                        sx={{ mb: 1 }}
                                    />
                                ) : (
                                    <Typography variant="h6" color="text.secondary">
                                        {profile.position || 'Должность не указана'}
                                    </Typography>
                                )}
                                
                                <Typography variant="body1" color="text.secondary">
                                    {profile.email}
                                </Typography>
                            </Box>
                            
                            <Box>
                                {editMode ? (
                                    <>
                                        <Button
                                            startIcon={<SaveIcon />}
                                            variant="contained"
                                            color="primary"
                                            onClick={handleSave}
                                            sx={{ mr: 1 }}
                                        >
                                            Сохранить
                                        </Button>
                                        <Button
                                            startIcon={<CancelIcon />}
                                            variant="outlined"
                                            onClick={handleEditToggle}
                                        >
                                            Отмена
                                        </Button>
                                    </>
                                ) : (
                                    <Button
                                        startIcon={<EditIcon />}
                                        variant="contained"
                                        onClick={handleEditToggle}
                                    >
                                        Редактировать
                                    </Button>
                                )}
                            </Box>
                        </Box>

                        <Divider sx={{ my: 2 }} />

                        {editMode ? (
                            <>
                                <TextField
                                    fullWidth
                                    label="Компания"
                                    value={editData.company || ''}
                                    onChange={handleInputChange('company')}
                                    size="small"
                                    sx={{ mb: 2 }}
                                />
                                <TextField
                                    fullWidth
                                    label="Местоположение"
                                    value={editData.location || ''}
                                    onChange={handleInputChange('location')}
                                    size="small"
                                    sx={{ mb: 2 }}
                                />
                                <TextField
                                    fullWidth
                                    multiline
                                    rows={3}
                                    label="О себе"
                                    value={editData.bio || ''}
                                    onChange={handleInputChange('bio')}
                                />
                            </>
                        ) : (
                            <>
                                <Typography variant="body2">
                                    <strong>Компания:</strong> {profile.company || 'Не указана'}
                                </Typography>
                                <Typography variant="body2">
                                    <strong>Местоположение:</strong> {profile.location || 'Не указано'}
                                </Typography>
                                <Typography variant="body2" sx={{ mt: 1 }}>
                                    {profile.bio || 'Описание отсутствует'}
                                </Typography>
                            </>
                        )}
                    </Box>
                </Box>
            </Paper>

            {/* Платформы */}
            {profile.platforms && profile.platforms.length > 0 && (
                <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
                    <Typography variant="h6" gutterBottom>
                        Подключенные платформы
                    </Typography>
                    <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                        {profile.platforms.map((platform, index) => (
                            <Chip key={index} label={platform} color="primary" variant="outlined" />
                        ))}
                    </Box>
                </Paper>
            )}

            {/* Список CV */}
            <Paper elevation={3} sx={{ p: 3 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h6">
                        Мои CV ({cvList.length})
                    </Typography>
                    <Button
                        startIcon={<CloudUploadIcon />}
                        variant="contained"
                        color="secondary"
                    >
                        Загрузить CV
                    </Button>
                </Box>

                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                    {cvList.length === 0 ? (
                        <Box sx={{ width: '100%' }}>
                            <Alert severity="info">
                                У вас пока нет загруженных CV. Начните с загрузки вашего первого резюме!
                            </Alert>
                        </Box>
                    ) : (
                        cvList.map((cv) => (
                            <Box key={cv.id} sx={{ flex: { xs: '1 1 100%', md: '1 1 calc(50% - 8px)' } }}>
                                <Card variant="outlined">
                                    <CardContent>
                                        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start' }}>
                                            <Box>
                                                <Typography variant="h6" gutterBottom>
                                                    {cv.title}
                                                </Typography>
                                                <Chip 
                                                    label={cv.platform} 
                                                    size="small" 
                                                    color="primary" 
                                                    sx={{ mb: 1 }}
                                                />
                                                <Typography variant="caption" display="block" color="text.secondary">
                                                    Создано: {new Date(cv.createdAt).toLocaleDateString()}
                                                </Typography>
                                                <Typography variant="caption" display="block" color="text.secondary">
                                                    Обновлено: {new Date(cv.updatedAt).toLocaleDateString()}
                                                </Typography>
                                            </Box>
                                            <Chip 
                                                label={cv.isActive ? 'Активно' : 'Неактивно'} 
                                                color={cv.isActive ? 'success' : 'default'}
                                                size="small"
                                            />
                                        </Box>
                                    </CardContent>
                                </Card>
                            </Box>
                        ))
                    )}
                </Box>
            </Paper>

            {/* Уведомления */}
            <Snackbar 
                open={openSnackbar} 
                autoHideDuration={6000} 
                onClose={handleCloseSnackbar}
            >
                <Alert 
                    onClose={handleCloseSnackbar} 
                    severity={error ? 'error' : 'success'} 
                    sx={{ width: '100%' }}
                >
                    {error || success}
                </Alert>
            </Snackbar>
        </Container>
    );
};

export default Profile;