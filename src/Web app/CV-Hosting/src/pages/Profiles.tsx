// src/pages/Profiles.tsx
import { useState, useEffect } from 'react';
import {
    Container,
    Box,
    Typography,
    Paper,
    TextField,
    Button,
    Card,
    CardContent,
    CardActions,
    Avatar,
    Chip,
    CircularProgress,
    Alert,
    InputAdornment,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Slider,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Pagination
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material';
import {
    Search as SearchIcon,
    FilterList as FilterListIcon,
    Work as WorkIcon,
    School as SchoolIcon,
    Language as LanguageIcon,
    Star as StarIcon,
    LocationOn as LocationOnIcon,
    ExpandMore as ExpandMoreIcon,
    Email as EmailIcon,
    Phone as PhoneIcon
} from '@mui/icons-material';
import axiosInstance from '../axiosConfig';

// Интерфейс профиля кандидата
interface CandidateProfile {
    id: string;
    firstName: string;
    lastName: string;
    middleName: string | null;
    email: string;
    phone: string;
    city: string;
    country: string;
    photoUrl: string | null;
    createdAt: string;
    lastModifiedAt: string;
    educationCount: number;
    workExperienceCount: number;
    skillsCount: number;
    languagesCount: number;
    currentPosition: string;
    currentCompany: string;
    salaryAmount: number;
    salaryCurrency: string;
    employmentType: string;
}

// Интерфейс фильтров
interface Filters {
    search: string;
    country: string;
    city: string;
    minExperience: number;
    maxExperience: number;
    minSalary: number;
    maxSalary: number;
    employmentType: string;
    minEducation: number;
}

const Profiles = () => {
    const [profiles, setProfiles] = useState<CandidateProfile[]>([]);
    const [filteredProfiles, setFilteredProfiles] = useState<CandidateProfile[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [page, setPage] = useState(1);
    const [showFilters, setShowFilters] = useState(false);
    const profilesPerPage = 9;

    // Состояние фильтров
    const [filters, setFilters] = useState<Filters>({
        search: '',
        country: '',
        city: '',
        minExperience: 0,
        maxExperience: 20,
        minSalary: 0,
        maxSalary: 10000,
        employmentType: '',
        minEducation: 0,
    });

    // Загрузка профилей
    useEffect(() => {
        fetchProfiles();
    }, []);

    // Применение фильтров
    useEffect(() => {
        applyFilters();
    }, [profiles, filters]);

    const fetchProfiles = async () => {
        try {
            setLoading(true);
            const response = await axiosInstance.get<CandidateProfile[]>('/api/profiles/api/Profiles/all');
            setProfiles(response.data);
            setFilteredProfiles(response.data);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка при загрузке профилей');
            console.error('Ошибка при загрузке профилей:', err);
        } finally {
            setLoading(false);
        }
    };

    const applyFilters = () => {
        let filtered = [...profiles];

        // Поиск по имени, должности, компании
        if (filters.search) {
            const searchLower = filters.search.toLowerCase();
            filtered = filtered.filter(
                (profile) =>
                    profile.firstName.toLowerCase().includes(searchLower) ||
                    profile.lastName.toLowerCase().includes(searchLower) ||
                    profile.currentPosition.toLowerCase().includes(searchLower) ||
                    profile.currentCompany.toLowerCase().includes(searchLower) ||
                    profile.email.toLowerCase().includes(searchLower)
            );
        }

        // Фильтр по стране
        if (filters.country) {
            filtered = filtered.filter((profile) => profile.country === filters.country);
        }

        // Фильтр по городу
        if (filters.city) {
            filtered = filtered.filter((profile) => profile.city === filters.city);
        }

        // Фильтр по опыту работы
        filtered = filtered.filter(
            (profile) =>
                profile.workExperienceCount >= filters.minExperience &&
                profile.workExperienceCount <= filters.maxExperience
        );

        // Фильтр по зарплате
        filtered = filtered.filter(
            (profile) =>
                profile.salaryAmount >= filters.minSalary &&
                profile.salaryAmount <= filters.maxSalary
        );

        // Фильтр по типу занятости
        if (filters.employmentType) {
            filtered = filtered.filter((profile) => profile.employmentType === filters.employmentType);
        }

        // Фильтр по образованию
        filtered = filtered.filter((profile) => profile.educationCount >= filters.minEducation);

        setFilteredProfiles(filtered);
        setPage(1); // Сброс на первую страницу при изменении фильтров
    };

    const handleFilterChange = (field: keyof Filters, value: any) => {
        setFilters({ ...filters, [field]: value });
    };

    const handleResetFilters = () => {
        setFilters({
            search: '',
            country: '',
            city: '',
            minExperience: 0,
            maxExperience: 20,
            minSalary: 0,
            maxSalary: 10000,
            employmentType: '',
            minEducation: 0,
        });
    };

    const handlePageChange = (_event: React.ChangeEvent<unknown>, value: number) => {
        setPage(value);
        window.scrollTo({ top: 0, behavior: 'smooth' });
    };

    // Получение уникальных стран и городов
    const uniqueCountries = Array.from(new Set(profiles.map((p) => p.country))).filter(Boolean);
    const uniqueCities = Array.from(new Set(profiles.map((p) => p.city))).filter(Boolean);

    // Пагинация
    const indexOfLastProfile = page * profilesPerPage;
    const indexOfFirstProfile = indexOfLastProfile - profilesPerPage;
    const currentProfiles = filteredProfiles.slice(indexOfFirstProfile, indexOfLastProfile);
    const totalPages = Math.ceil(filteredProfiles.length / profilesPerPage);

    if (loading) {
        return (
            <Container maxWidth="xl">
                <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '80vh' }}>
                    <CircularProgress />
                </Box>
            </Container>
        );
    }

    return (
        <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
            {/* Заголовок и поиск */}
            <Box sx={{ mb: 4 }}>
                <Typography variant="h4" gutterBottom fontWeight="bold">
                    Кандидаты
                </Typography>
                <Typography variant="body1" color="text.secondary" gutterBottom>
                    Найдено кандидатов: {filteredProfiles.length}
                </Typography>

                <Box sx={{ mt: 3, display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                    <TextField
                        placeholder="Поиск по имени, должности, компании..."
                        variant="outlined"
                        value={filters.search}
                        onChange={(e) => handleFilterChange('search', e.target.value)}
                        sx={{ flex: 1, minWidth: 300 }}
                        InputProps={{
                            startAdornment: (
                                <InputAdornment position="start">
                                    <SearchIcon />
                                </InputAdornment>
                            ),
                        }}
                    />
                    <Button
                        variant={showFilters ? 'contained' : 'outlined'}
                        startIcon={<FilterListIcon />}
                        onClick={() => setShowFilters(!showFilters)}
                    >
                        Фильтры
                    </Button>
                </Box>
            </Box>

            {/* Панель фильтров */}
            {showFilters && (
                <Paper sx={{ p: 3, mb: 4 }}>
                    <Typography variant="h6" gutterBottom>
                        Фильтры
                    </Typography>

                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
                        {/* Локация */}
                        <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                            <FormControl sx={{ minWidth: 200 }}>
                                <InputLabel>Страна</InputLabel>
                                <Select
                                    value={filters.country}
                                    label="Страна"
                                    onChange={(e: SelectChangeEvent) => handleFilterChange('country', e.target.value)}
                                >
                                    <MenuItem value="">Все страны</MenuItem>
                                    {uniqueCountries.map((country) => (
                                        <MenuItem key={country} value={country}>
                                            {country}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>

                            <FormControl sx={{ minWidth: 200 }}>
                                <InputLabel>Город</InputLabel>
                                <Select
                                    value={filters.city}
                                    label="Город"
                                    onChange={(e: SelectChangeEvent) => handleFilterChange('city', e.target.value)}
                                >
                                    <MenuItem value="">Все города</MenuItem>
                                    {uniqueCities.map((city) => (
                                        <MenuItem key={city} value={city}>
                                            {city}
                                        </MenuItem>
                                    ))}
                                </Select>
                            </FormControl>

                            <FormControl sx={{ minWidth: 200 }}>
                                <InputLabel>Тип занятости</InputLabel>
                                <Select
                                    value={filters.employmentType}
                                    label="Тип занятости"
                                    onChange={(e: SelectChangeEvent) => handleFilterChange('employmentType', e.target.value)}
                                >
                                    <MenuItem value="">Все типы</MenuItem>
                                    <MenuItem value="Full-time">Full-time</MenuItem>
                                    <MenuItem value="Part-time">Part-time</MenuItem>
                                    <MenuItem value="Contract">Contract</MenuItem>
                                    <MenuItem value="Freelance">Freelance</MenuItem>
                                </Select>
                            </FormControl>
                        </Box>

                        {/* Опыт работы */}
                        <Box>
                            <Typography gutterBottom>
                                Опыт работы: {filters.minExperience} - {filters.maxExperience} лет
                            </Typography>
                            <Slider
                                value={[filters.minExperience, filters.maxExperience]}
                                onChange={(_e, newValue) => {
                                    const [min, max] = newValue as number[];
                                    setFilters({ ...filters, minExperience: min, maxExperience: max });
                                }}
                                valueLabelDisplay="auto"
                                min={0}
                                max={20}
                                marks={[
                                    { value: 0, label: '0' },
                                    { value: 5, label: '5' },
                                    { value: 10, label: '10' },
                                    { value: 15, label: '15' },
                                    { value: 20, label: '20+' },
                                ]}
                            />
                        </Box>

                        {/* Зарплата */}
                        <Box>
                            <Typography gutterBottom>
                                Зарплата: ${filters.minSalary} - ${filters.maxSalary}
                            </Typography>
                            <Slider
                                value={[filters.minSalary, filters.maxSalary]}
                                onChange={(_e, newValue) => {
                                    const [min, max] = newValue as number[];
                                    setFilters({ ...filters, minSalary: min, maxSalary: max });
                                }}
                                valueLabelDisplay="auto"
                                min={0}
                                max={10000}
                                step={500}
                                marks={[
                                    { value: 0, label: '$0' },
                                    { value: 2500, label: '$2.5k' },
                                    { value: 5000, label: '$5k' },
                                    { value: 7500, label: '$7.5k' },
                                    { value: 10000, label: '$10k+' },
                                ]}
                            />
                        </Box>

                        {/* Минимальное образование */}
                        <Box>
                            <Typography gutterBottom>
                                Минимальное количество образований: {filters.minEducation}
                            </Typography>
                            <Slider
                                value={filters.minEducation}
                                onChange={(_e, newValue) => handleFilterChange('minEducation', newValue as number)}
                                valueLabelDisplay="auto"
                                min={0}
                                max={5}
                                marks
                            />
                        </Box>

                        {/* Кнопки управления */}
                        <Box sx={{ display: 'flex', gap: 2 }}>
                            <Button variant="outlined" onClick={handleResetFilters}>
                                Сбросить фильтры
                            </Button>
                        </Box>
                    </Box>
                </Paper>
            )}

            {/* Список профилей */}
            {error && (
                <Alert severity="error" sx={{ mb: 3 }}>
                    {error}
                </Alert>
            )}

            {filteredProfiles.length === 0 ? (
                <Paper sx={{ p: 4, textAlign: 'center' }}>
                    <Typography variant="h6" color="text.secondary">
                        Кандидаты не найдены
                    </Typography>
                    <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                        Попробуйте изменить параметры поиска
                    </Typography>
                </Paper>
            ) : (
                <>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 3, mb: 4 }}>
                        {currentProfiles.map((profile) => (
                            <Card
                                key={profile.id}
                                sx={{
                                    width: { xs: '100%', sm: 'calc(50% - 12px)', md: 'calc(33.333% - 16px)' },
                                    display: 'flex',
                                    flexDirection: 'column',
                                    transition: 'transform 0.2s, box-shadow 0.2s',
                                    '&:hover': {
                                        transform: 'translateY(-4px)',
                                        boxShadow: 6,
                                    },
                                }}
                            >
                                <CardContent sx={{ flexGrow: 1 }}>
                                    {/* Аватар и имя */}
                                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                                        <Avatar
                                            src={profile.photoUrl || undefined}
                                            sx={{ width: 60, height: 60, mr: 2 }}
                                        >
                                            {profile.firstName[0]}
                                            {profile.lastName[0]}
                                        </Avatar>
                                        <Box sx={{ flex: 1 }}>
                                            <Typography variant="h6" fontWeight="bold">
                                                {profile.firstName} {profile.lastName}
                                            </Typography>
                                            <Typography variant="body2" color="text.secondary">
                                                {profile.currentPosition}
                                            </Typography>
                                        </Box>
                                    </Box>

                                    {/* Текущая компания */}
                                    {profile.currentCompany && (
                                        <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                            <WorkIcon sx={{ fontSize: 18, mr: 1, color: 'text.secondary' }} />
                                            <Typography variant="body2">{profile.currentCompany}</Typography>
                                        </Box>
                                    )}

                                    {/* Локация */}
                                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                        <LocationOnIcon sx={{ fontSize: 18, mr: 1, color: 'text.secondary' }} />
                                        <Typography variant="body2">
                                            {profile.city}, {profile.country}
                                        </Typography>
                                    </Box>

                                    {/* Контакты */}
                                    <Accordion sx={{ mt: 2, boxShadow: 'none', '&:before': { display: 'none' } }}>
                                        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                                            <Typography variant="body2" color="primary">
                                                Контактная информация
                                            </Typography>
                                        </AccordionSummary>
                                        <AccordionDetails>
                                            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                                    <EmailIcon sx={{ fontSize: 16, mr: 1, color: 'text.secondary' }} />
                                                    <Typography variant="body2">{profile.email}</Typography>
                                                </Box>
                                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                                    <PhoneIcon sx={{ fontSize: 16, mr: 1, color: 'text.secondary' }} />
                                                    <Typography variant="body2">{profile.phone}</Typography>
                                                </Box>
                                            </Box>
                                        </AccordionDetails>
                                    </Accordion>

                                    {/* Статистика */}
                                    <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap', mt: 2 }}>
                                        <Chip
                                            icon={<WorkIcon />}
                                            label={`${profile.workExperienceCount} опыта`}
                                            size="small"
                                            variant="outlined"
                                        />
                                        <Chip
                                            icon={<SchoolIcon />}
                                            label={`${profile.educationCount} образ.`}
                                            size="small"
                                            variant="outlined"
                                        />
                                        <Chip
                                            icon={<StarIcon />}
                                            label={`${profile.skillsCount} навыков`}
                                            size="small"
                                            variant="outlined"
                                        />
                                        <Chip
                                            icon={<LanguageIcon />}
                                            label={`${profile.languagesCount} языков`}
                                            size="small"
                                            variant="outlined"
                                        />
                                    </Box>

                                    {/* Зарплата */}
                                    {profile.salaryAmount > 0 && (
                                        <Box sx={{ mt: 2 }}>
                                            <Typography variant="h6" color="primary" fontWeight="bold">
                                                {profile.salaryCurrency}
                                                {profile.salaryAmount.toLocaleString()}
                                            </Typography>
                                            <Typography variant="caption" color="text.secondary">
                                                Ожидаемая зарплата
                                            </Typography>
                                        </Box>
                                    )}
                                </CardContent>

                                <CardActions>
                                    <Button size="small" variant="outlined" fullWidth>
                                        Просмотреть профиль
                                    </Button>
                                </CardActions>
                            </Card>
                        ))}
                    </Box>

                    {/* Пагинация */}
                    {totalPages > 1 && (
                        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
                            <Pagination
                                count={totalPages}
                                page={page}
                                onChange={handlePageChange}
                                color="primary"
                                size="large"
                                showFirstButton
                                showLastButton
                            />
                        </Box>
                    )}
                </>
            )}
        </Container>
    );
};

export default Profiles;
