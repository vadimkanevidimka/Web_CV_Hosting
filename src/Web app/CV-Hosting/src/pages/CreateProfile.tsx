// src/pages/CreateProfile.tsx
import { useState } from 'react';
import {
    Container,
    Box,
    Typography,
    Paper,
    Stepper,
    Step,
    StepLabel,
    Button,
    TextField,
    CircularProgress,
    Alert,
    Snackbar,
    Divider,
    IconButton,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Chip,
    Card,
    CardContent,
    InputAdornment
} from '@mui/material';
import {
    CloudUpload as CloudUploadIcon,
    Delete as DeleteIcon,
    Add as AddIcon,
    Edit as EditIcon,
    Check as CheckIcon
} from '@mui/icons-material';
import axiosInstance from '../axiosConfig';

// Интерфейсы для данных CV
interface Education {
    InstitutionName: string;
    Specialization: string;
    Degree: string;
    StartYear: number;
    EndYear: number;
}

interface WorkExperience {
    CompanyName: string;
    City: string;
    Position: string;
    StartDate: string;
    EndDate: string | null;
    IsCurrentlyWorking: boolean;
    JobDescription: string;
}

interface Skill {
    SkillName: string;
}

interface Language {
    LanguageName: string;
    ProficiencyLevel: string;
}

interface Citizenship {
    Country: string;
    City: string;
    State: string;
    UndergruondStation: string;
}

interface DriverLicense {
    Category: string;
}

interface PortfolioItem {
    ProjectName: string;
    Url: string;
    Description: string;
}

interface SalaryExpectation {
    Amount: number;
    Currency: string;
    Type: string;
}

interface EmploymentType {
    Type: string;
}

interface CVProfile {
    FirstName: string;
    LastName: string;
    MiddleName: string | null;
    Email: string;
    Phone: string;
    DateOfBirth: string | null;
    City: string;
    Country: string;
    PhotoUrl: string | null;
    Educations: Education[];
    WorkExperiences: WorkExperience[];
    Skills: Skill[];
    Languages: Language[];
    Citizenships: Citizenship[];
    DriverLicenses: DriverLicense[];
    PortfolioItems: PortfolioItem[];
    SalaryExpectations: SalaryExpectation | null;
    EmploymentType: EmploymentType | null;
}

const steps = ['Загрузка файла', 'Редактирование данных', 'Подтверждение'];

const CreateProfile = () => {
    const [activeStep, setActiveStep] = useState(0);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [uploading, setUploading] = useState(false);
    const [profileData, setProfileData] = useState<CVProfile | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const [openSnackbar, setOpenSnackbar] = useState(false);

    // Обработка выбора файла
    const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files[0]) {
            setSelectedFile(event.target.files[0]);
            setError(null);
        }
    };

    // Загрузка файла на сервер
    const handleFileUpload = async () => {
        if (!selectedFile) {
            setError('Пожалуйста, выберите файл');
            setOpenSnackbar(true);
            return;
        }

        setUploading(true);
        const formData = new FormData();
        formData.append('file', selectedFile);

        try {
            const response = await axiosInstance.post('/api/recognize/api/Documents/upload', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
                timeout: 300000, // 5 минут
            });

            // Преобразуем данные из API в нужный формат
            const apiData = response.data.applicantProfile;
            const transformedData: CVProfile = {
                FirstName: apiData.firstName || '',
                LastName: apiData.lastName || '',
                MiddleName: apiData.middleName,
                Email: apiData.email || '',
                Phone: apiData.phone || '',
                DateOfBirth: apiData.dateOfBirth,
                City: apiData.city || '',
                Country: apiData.country || '',
                PhotoUrl: apiData.photoUrl,
                Educations: (apiData.educations || []).map((edu: any) => ({
                    InstitutionName: edu.institutionName || '',
                    Specialization: edu.specialization || '',
                    Degree: edu.degree || '',
                    StartYear: edu.startYear || new Date().getFullYear(),
                    EndYear: edu.endYear || new Date().getFullYear(),
                })),
                WorkExperiences: (apiData.workExperiences || []).map((work: any) => ({
                    CompanyName: work.companyName || '',
                    City: work.city || '',
                    Position: work.position || '',
                    StartDate: work.startDate || '',
                    EndDate: work.endDate,
                    IsCurrentlyWorking: work.isCurrentlyWorking || false,
                    JobDescription: work.jobDescription || '',
                })),
                Skills: (apiData.skills || []).map((skill: any) => ({
                    SkillName: skill.skillName || '',
                })),
                Languages: (apiData.languages || []).map((lang: any) => ({
                    LanguageName: lang.languageName || '',
                    ProficiencyLevel: lang.proficiencyLevel || '',
                })),
                Citizenships: (apiData.citizenships || []).map((cit: any) => ({
                    Country: cit.country || '',
                    City: cit.city || '',
                    State: cit.state || '',
                    UndergruondStation: cit.undergroundStation || '',
                })),
                DriverLicenses: (apiData.driverLicenses || []).map((lic: any) => ({
                    Category: lic.category || '',
                })),
                PortfolioItems: (apiData.portfolioItems || []).map((item: any) => ({
                    ProjectName: item.projectName || '',
                    Url: item.url || '',
                    Description: item.description || '',
                })),
                SalaryExpectations: {
                    Amount: apiData.salaryExpectations?.amount || 0,
                    Currency: apiData.salaryExpectations?.currency || '',
                    Type: apiData.salaryExpectations?.type || '',
                },
                EmploymentType: {
                    Type: apiData.employmentType?.type || '',
                },
            };

            setProfileData(transformedData);
            setSuccess('Файл успешно обработан!');
            setOpenSnackbar(true);
            setActiveStep(1);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка при загрузке файла');
            setOpenSnackbar(true);
        } finally {
            setUploading(false);
        }
    };

    // Обновление основных данных профиля
    const handleProfileChange = (field: keyof CVProfile, value: any) => {
        if (profileData) {
            setProfileData({ ...profileData, [field]: value });
        }
    };

    // Добавление/удаление элементов массивов
    const handleAddEducation = () => {
        if (profileData) {
            setProfileData({
                ...profileData,
                Educations: [
                    ...profileData.Educations,
                    {
                        InstitutionName: '',
                        Specialization: '',
                        Degree: '',
                        StartYear: new Date().getFullYear(),
                        EndYear: new Date().getFullYear(),
                    },
                ],
            });
        }
    };

    const handleRemoveEducation = (index: number) => {
        if (profileData) {
            setProfileData({
                ...profileData,
                Educations: profileData.Educations.filter((_, i) => i !== index),
            });
        }
    };

    const handleEducationChange = (index: number, field: keyof Education, value: any) => {
        if (profileData) {
            const updatedEducations = [...profileData.Educations];
            updatedEducations[index] = { ...updatedEducations[index], [field]: value };
            setProfileData({ ...profileData, Educations: updatedEducations });
        }
    };

    // Работа с опытом работы
    const handleAddWorkExperience = () => {
        if (profileData) {
            setProfileData({
                ...profileData,
                WorkExperiences: [
                    ...profileData.WorkExperiences,
                    {
                        CompanyName: '',
                        City: '',
                        Position: '',
                        StartDate: '',
                        EndDate: null,
                        IsCurrentlyWorking: false,
                        JobDescription: '',
                    },
                ],
            });
        }
    };

    const handleRemoveWorkExperience = (index: number) => {
        if (profileData) {
            setProfileData({
                ...profileData,
                WorkExperiences: profileData.WorkExperiences.filter((_, i) => i !== index),
            });
        }
    };

    const handleWorkExperienceChange = (index: number, field: keyof WorkExperience, value: any) => {
        if (profileData) {
            const updatedExperiences = [...profileData.WorkExperiences];
            updatedExperiences[index] = { ...updatedExperiences[index], [field]: value };
            setProfileData({ ...profileData, WorkExperiences: updatedExperiences });
        }
    };

    // Работа с навыками
    const handleAddSkill = () => {
        if (profileData) {
            setProfileData({
                ...profileData,
                Skills: [...profileData.Skills, { SkillName: '' }],
            });
        }
    };

    const handleRemoveSkill = (index: number) => {
        if (profileData) {
            setProfileData({
                ...profileData,
                Skills: profileData.Skills.filter((_, i) => i !== index),
            });
        }
    };

    const handleSkillChange = (index: number, value: string) => {
        if (profileData) {
            const updatedSkills = [...profileData.Skills];
            updatedSkills[index] = { SkillName: value };
            setProfileData({ ...profileData, Skills: updatedSkills });
        }
    };

    // Работа с языками
    const handleAddLanguage = () => {
        if (profileData) {
            setProfileData({
                ...profileData,
                Languages: [...profileData.Languages, { LanguageName: '', ProficiencyLevel: '' }],
            });
        }
    };

    const handleRemoveLanguage = (index: number) => {
        if (profileData) {
            setProfileData({
                ...profileData,
                Languages: profileData.Languages.filter((_, i) => i !== index),
            });
        }
    };

    const handleLanguageChange = (index: number, field: keyof Language, value: string) => {
        if (profileData) {
            const updatedLanguages = [...profileData.Languages];
            updatedLanguages[index] = { ...updatedLanguages[index], [field]: value };
            setProfileData({ ...profileData, Languages: updatedLanguages });
        }
    };

    // Финальная отправка данных
    const handleSubmit = async () => {
        setUploading(true);
        try {
            await axiosInstance.post('/api/cv/create', profileData);
            setSuccess('Профиль успешно создан!');
            setOpenSnackbar(true);
            setActiveStep(2);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Ошибка при создании профиля');
            setOpenSnackbar(true);
        } finally {
            setUploading(false);
        }
    };

    const handleBack = () => {
        setActiveStep((prevStep) => prevStep - 1);
    };

    const handleNext = () => {
        if (activeStep === 1) {
            setActiveStep(2);
        }
    };

    const handleCloseSnackbar = () => {
        setOpenSnackbar(false);
        setError(null);
        setSuccess(null);
    };

    // Рендер этапа 1: Загрузка файла
    const renderUploadStep = () => (
        <Box sx={{ textAlign: 'center', py: 4 }}>
            <CloudUploadIcon sx={{ fontSize: 80, color: 'primary.main', mb: 2 }} />
            <Typography variant="h5" gutterBottom>
                Загрузите ваше резюме
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 4 }}>
                Поддерживаемые форматы: PDF, DOC, DOCX
            </Typography>

            <Button
                variant="outlined"
                component="label"
                startIcon={<CloudUploadIcon />}
                sx={{ mb: 2 }}
            >
                Выбрать файл
                <input type="file" hidden accept=".pdf,.doc,.docx" onChange={handleFileSelect} />
            </Button>

            {selectedFile && (
                <Box sx={{ mt: 2 }}>
                    <Chip
                        label={selectedFile.name}
                        onDelete={() => setSelectedFile(null)}
                        color="primary"
                        variant="outlined"
                    />
                </Box>
            )}

            <Box sx={{ mt: 4 }}>
                <Button
                    variant="contained"
                    onClick={handleFileUpload}
                    disabled={!selectedFile || uploading}
                    size="large"
                >
                    {uploading ? <CircularProgress size={24} /> : 'Загрузить и обработать'}
                </Button>
            </Box>
        </Box>
    );

    // Рендер этапа 2: Редактирование данных
    const renderEditStep = () => {
        if (!profileData) return null;

        return (
            <Box sx={{ py: 3 }}>
                {/* Основная информация */}
                <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <EditIcon /> Основная информация
                </Typography>
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2, mb: 4 }}>
                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Имя"
                            value={profileData.FirstName}
                            onChange={(e) => handleProfileChange('FirstName', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Фамилия"
                            value={profileData.LastName}
                            onChange={(e) => handleProfileChange('LastName', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Отчество"
                            value={profileData.MiddleName || ''}
                            onChange={(e) => handleProfileChange('MiddleName', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Email"
                            type="email"
                            value={profileData.Email}
                            onChange={(e) => handleProfileChange('Email', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Телефон"
                            value={profileData.Phone}
                            onChange={(e) => handleProfileChange('Phone', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Дата рождения"
                            type="date"
                            InputLabelProps={{ shrink: true }}
                            value={profileData.DateOfBirth || ''}
                            onChange={(e) => handleProfileChange('DateOfBirth', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Город"
                            value={profileData.City}
                            onChange={(e) => handleProfileChange('City', e.target.value)}
                        />
                    </Box>
                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '200px' }}>
                        <TextField
                            fullWidth
                            label="Страна"
                            value={profileData.Country}
                            onChange={(e) => handleProfileChange('Country', e.target.value)}
                        />
                    </Box>
                </Box>

                <Divider sx={{ my: 3 }} />

                {/* Образование */}
                <Box sx={{ mb: 4 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="h6">Образование</Typography>
                        <Button startIcon={<AddIcon />} onClick={handleAddEducation} variant="outlined" size="small">
                            Добавить
                        </Button>
                    </Box>
                    {profileData.Educations.map((edu, index) => (
                        <Card key={index} sx={{ mb: 2 }}>
                            <CardContent>
                                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
                                    <Typography variant="subtitle2" color="primary">
                                        Образование #{index + 1}
                                    </Typography>
                                    <IconButton size="small" onClick={() => handleRemoveEducation(index)}>
                                        <DeleteIcon />
                                    </IconButton>
                                </Box>
                                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                                        <TextField
                                            fullWidth
                                            label="Учебное заведение"
                                            value={edu.InstitutionName}
                                            onChange={(e) => handleEducationChange(index, 'InstitutionName', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                                        <TextField
                                            fullWidth
                                            label="Специализация"
                                            value={edu.Specialization}
                                            onChange={(e) => handleEducationChange(index, 'Specialization', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '150px' }}>
                                        <TextField
                                            fullWidth
                                            label="Степень"
                                            value={edu.Degree}
                                            onChange={(e) => handleEducationChange(index, 'Degree', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '150px' }}>
                                        <TextField
                                            fullWidth
                                            label="Год начала"
                                            type="number"
                                            value={edu.StartYear}
                                            onChange={(e) => handleEducationChange(index, 'StartYear', parseInt(e.target.value))}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '150px' }}>
                                        <TextField
                                            fullWidth
                                            label="Год окончания"
                                            type="number"
                                            value={edu.EndYear}
                                            onChange={(e) => handleEducationChange(index, 'EndYear', parseInt(e.target.value))}
                                            size="small"
                                        />
                                    </Box>
                                </Box>
                            </CardContent>
                        </Card>
                    ))}
                </Box>

                <Divider sx={{ my: 3 }} />

                {/* Опыт работы */}
                <Box sx={{ mb: 4 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="h6">Опыт работы</Typography>
                        <Button startIcon={<AddIcon />} onClick={handleAddWorkExperience} variant="outlined" size="small">
                            Добавить
                        </Button>
                    </Box>
                    {profileData.WorkExperiences.map((work, index) => (
                        <Card key={index} sx={{ mb: 2 }}>
                            <CardContent>
                                <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
                                    <Typography variant="subtitle2" color="primary">
                                        Опыт #{index + 1}
                                    </Typography>
                                    <IconButton size="small" onClick={() => handleRemoveWorkExperience(index)}>
                                        <DeleteIcon />
                                    </IconButton>
                                </Box>
                                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                                        <TextField
                                            fullWidth
                                            label="Компания"
                                            value={work.CompanyName}
                                            onChange={(e) => handleWorkExperienceChange(index, 'CompanyName', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                                        <TextField
                                            fullWidth
                                            label="Город"
                                            value={work.City}
                                            onChange={(e) => handleWorkExperienceChange(index, 'City', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 100%' }}>
                                        <TextField
                                            fullWidth
                                            label="Должность"
                                            value={work.Position}
                                            onChange={(e) => handleWorkExperienceChange(index, 'Position', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                                        <TextField
                                            fullWidth
                                            label="Дата начала"
                                            type="date"
                                            InputLabelProps={{ shrink: true }}
                                            value={work.StartDate}
                                            onChange={(e) => handleWorkExperienceChange(index, 'StartDate', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 calc(50% - 8px)', minWidth: '200px' }}>
                                        <TextField
                                            fullWidth
                                            label="Дата окончания"
                                            type="date"
                                            InputLabelProps={{ shrink: true }}
                                            value={work.EndDate || ''}
                                            onChange={(e) => handleWorkExperienceChange(index, 'EndDate', e.target.value)}
                                            size="small"
                                            disabled={work.IsCurrentlyWorking}
                                        />
                                    </Box>
                                    <Box sx={{ flex: '1 1 100%' }}>
                                        <TextField
                                            fullWidth
                                            label="Описание работы"
                                            multiline
                                            rows={3}
                                            value={work.JobDescription}
                                            onChange={(e) => handleWorkExperienceChange(index, 'JobDescription', e.target.value)}
                                            size="small"
                                        />
                                    </Box>
                                </Box>
                            </CardContent>
                        </Card>
                    ))}
                </Box>

                <Divider sx={{ my: 3 }} />

                {/* Навыки */}
                <Box sx={{ mb: 4 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="h6">Навыки</Typography>
                        <Button startIcon={<AddIcon />} onClick={handleAddSkill} variant="outlined" size="small">
                            Добавить
                        </Button>
                    </Box>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
                        {profileData.Skills.map((skill, index) => (
                            <TextField
                                key={index}
                                label={`Навык ${index + 1}`}
                                value={skill.SkillName}
                                onChange={(e) => handleSkillChange(index, e.target.value)}
                                size="small"
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton size="small" onClick={() => handleRemoveSkill(index)}>
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                }}
                            />
                        ))}
                    </Box>
                </Box>

                <Divider sx={{ my: 3 }} />

                {/* Языки */}
                <Box sx={{ mb: 4 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="h6">Языки</Typography>
                        <Button startIcon={<AddIcon />} onClick={handleAddLanguage} variant="outlined" size="small">
                            Добавить
                        </Button>
                    </Box>
                    {profileData.Languages.map((lang, index) => (
                        <Box key={index} sx={{ display: 'flex', gap: 2, mb: 2, alignItems: 'center' }}>
                            <TextField
                                label="Язык"
                                value={lang.LanguageName}
                                onChange={(e) => handleLanguageChange(index, 'LanguageName', e.target.value)}
                                size="small"
                                sx={{ flex: 1 }}
                            />
                            <FormControl size="small" sx={{ flex: 1 }}>
                                <InputLabel>Уровень</InputLabel>
                                <Select
                                    value={lang.ProficiencyLevel}
                                    label="Уровень"
                                    onChange={(e) => handleLanguageChange(index, 'ProficiencyLevel', e.target.value)}
                                >
                                    <MenuItem value="A1">A1 - Начальный</MenuItem>
                                    <MenuItem value="A2">A2 - Элементарный</MenuItem>
                                    <MenuItem value="B1">B1 - Средний</MenuItem>
                                    <MenuItem value="B2">B2 - Выше среднего</MenuItem>
                                    <MenuItem value="C1">C1 - Продвинутый</MenuItem>
                                    <MenuItem value="C2">C2 - Владение в совершенстве</MenuItem>
                                    <MenuItem value="Native">Родной</MenuItem>
                                </Select>
                            </FormControl>
                            <IconButton onClick={() => handleRemoveLanguage(index)}>
                                <DeleteIcon />
                            </IconButton>
                        </Box>
                    ))}
                </Box>

                {/* Зарплатные ожидания */}
                {profileData.SalaryExpectations && (
                    <>
                        <Divider sx={{ my: 3 }} />
                        <Box sx={{ mb: 4 }}>
                            <Typography variant="h6" gutterBottom>
                                Зарплатные ожидания
                            </Typography>
                            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                                <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '150px' }}>
                                    <TextField
                                        fullWidth
                                        label="Сумма"
                                        type="number"
                                        value={profileData.SalaryExpectations.Amount}
                                        onChange={(e) =>
                                            handleProfileChange('SalaryExpectations', {
                                                ...profileData.SalaryExpectations,
                                                Amount: parseFloat(e.target.value),
                                            })
                                        }
                                        size="small"
                                    />
                                </Box>
                                <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '150px' }}>
                                    <TextField
                                        fullWidth
                                        label="Валюта"
                                        value={profileData.SalaryExpectations.Currency}
                                        onChange={(e) =>
                                            handleProfileChange('SalaryExpectations', {
                                                ...profileData.SalaryExpectations,
                                                Currency: e.target.value,
                                            })
                                        }
                                        size="small"
                                    />
                                </Box>
                                <Box sx={{ flex: '1 1 calc(33.333% - 16px)', minWidth: '150px' }}>
                                    <FormControl fullWidth size="small">
                                        <InputLabel>Тип</InputLabel>
                                        <Select
                                            value={profileData.SalaryExpectations.Type}
                                            label="Тип"
                                            onChange={(e) =>
                                                handleProfileChange('SalaryExpectations', {
                                                    ...profileData.SalaryExpectations,
                                                    Type: e.target.value,
                                                })
                                            }
                                        >
                                            <MenuItem value="Net">Net (на руки)</MenuItem>
                                            <MenuItem value="Gross">Gross (до вычета налогов)</MenuItem>
                                        </Select>
                                    </FormControl>
                                </Box>
                            </Box>
                        </Box>
                    </>
                )}
            </Box>
        );
    };

    // Рендер этапа 3: Подтверждение
    const renderConfirmStep = () => {
        if (!profileData) return null;

        return (
            <Box sx={{ py: 3 }}>
                <Alert severity="info" sx={{ mb: 3 }}>
                    Пожалуйста, проверьте все данные перед отправкой
                </Alert>

                <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
                    <Typography variant="h6" gutterBottom>
                        Основная информация
                    </Typography>
                    <Typography variant="body2">
                        <strong>ФИО:</strong> {profileData.FirstName} {profileData.MiddleName} {profileData.LastName}
                    </Typography>
                    <Typography variant="body2">
                        <strong>Email:</strong> {profileData.Email}
                    </Typography>
                    <Typography variant="body2">
                        <strong>Телефон:</strong> {profileData.Phone}
                    </Typography>
                    <Typography variant="body2">
                        <strong>Местоположение:</strong> {profileData.City}, {profileData.Country}
                    </Typography>
                </Paper>

                <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
                    <Typography variant="h6" gutterBottom>
                        Образование ({profileData.Educations.length})
                    </Typography>
                    {profileData.Educations.map((edu, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                            <Typography variant="body2">
                                <strong>{edu.InstitutionName}</strong> - {edu.Specialization} ({edu.Degree})
                            </Typography>
                            <Typography variant="caption" color="text.secondary">
                                {edu.StartYear} - {edu.EndYear}
                            </Typography>
                        </Box>
                    ))}
                </Paper>

                <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
                    <Typography variant="h6" gutterBottom>
                        Опыт работы ({profileData.WorkExperiences.length})
                    </Typography>
                    {profileData.WorkExperiences.map((work, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                            <Typography variant="body2">
                                <strong>{work.Position}</strong> в {work.CompanyName}
                            </Typography>
                            <Typography variant="caption" color="text.secondary">
                                {work.StartDate} - {work.EndDate || 'настоящее время'}
                            </Typography>
                        </Box>
                    ))}
                </Paper>

                <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
                    <Typography variant="h6" gutterBottom>
                        Навыки ({profileData.Skills.length})
                    </Typography>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
                        {profileData.Skills.map((skill, index) => (
                            <Chip key={index} label={skill.SkillName} color="primary" variant="outlined" />
                        ))}
                    </Box>
                </Paper>

                <Paper elevation={2} sx={{ p: 3 }}>
                    <Typography variant="h6" gutterBottom>
                        Языки ({profileData.Languages.length})
                    </Typography>
                    {profileData.Languages.map((lang, index) => (
                        <Typography key={index} variant="body2">
                            {lang.LanguageName} - {lang.ProficiencyLevel}
                        </Typography>
                    ))}
                </Paper>

                <Box sx={{ mt: 4, textAlign: 'center' }}>
                    <Button
                        variant="contained"
                        size="large"
                        startIcon={<CheckIcon />}
                        onClick={handleSubmit}
                        disabled={uploading}
                    >
                        {uploading ? <CircularProgress size={24} /> : 'Подтвердить и создать профиль'}
                    </Button>
                </Box>
            </Box>
        );
    };

    return (
        <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
            <Typography variant="h4" gutterBottom align="center">
                Создание профиля из резюме
            </Typography>

            <Paper elevation={3} sx={{ p: 3, mt: 3 }}>
                <Stepper activeStep={activeStep} sx={{ mb: 4 }}>
                    {steps.map((label) => (
                        <Step key={label}>
                            <StepLabel>{label}</StepLabel>
                        </Step>
                    ))}
                </Stepper>

                {activeStep === 0 && renderUploadStep()}
                {activeStep === 1 && renderEditStep()}
                {activeStep === 2 && renderConfirmStep()}

                {activeStep > 0 && activeStep < 2 && (
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 3 }}>
                        <Button onClick={handleBack} disabled={uploading}>
                            Назад
                        </Button>
                        <Button variant="contained" onClick={handleNext} disabled={uploading}>
                            Далее
                        </Button>
                    </Box>
                )}
            </Paper>

            <Snackbar open={openSnackbar} autoHideDuration={6000} onClose={handleCloseSnackbar}>
                <Alert onClose={handleCloseSnackbar} severity={error ? 'error' : 'success'} sx={{ width: '100%' }}>
                    {error || success}
                </Alert>
            </Snackbar>
        </Container>
    );
};

export default CreateProfile;
