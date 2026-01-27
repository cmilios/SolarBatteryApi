import axios from 'axios';
import type {
    ApplicationDto,
    CreateApplicationCommand,
    FileDto,
    ConcurrencyCalculationDto,
    CreateConcurrencyCalculationCommand
} from '../types/api';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:7245/api';

const api = axios.create({
    baseURL: API_BASE_URL,
});

export const applicationService = {
    getAll: async () => {
        const response = await api.get<ApplicationDto[]>('/Application');
        return response.data;
    },
    getById: async (id: number) => {
        const response = await api.get<ApplicationDto>(`/Application/${id}`);
        return response.data;
    },
    create: async (command: CreateApplicationCommand) => {
        const response = await api.post<ApplicationDto>('/Application', command);
        return response.data;
    },
    calculate: async (id: number, command: CreateConcurrencyCalculationCommand) => {
        const response = await api.post<ConcurrencyCalculationDto>(`/Application/${id}/calculate`, command);
        return response.data;
    }
};

export const fileService = {
    getAll: async () => {
        const response = await api.get<FileDto[]>('/File');
        return response.data;
    },
    getById: async (id: number) => {
        const response = await api.get<FileDto>(`/File/${id}`);
        return response.data;
    },
    upload: async (file: File, type: number, applicationId: number) => {
        const formData = new FormData();
        formData.append('File', file);
        formData.append('Type', type.toString());
        formData.append('ApplicationId', applicationId.toString());

        const response = await api.put<FileDto>('/File/upload', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        });
        return response.data;
    },
    parse: async (id: number) => {
        const response = await api.post(`/File/${id}/parse`);
        return response.data;
    },
    download: async (id: number) => {
        const response = await api.get(`/File/${id}/download`, {
            responseType: 'blob',
        });
        return response.data;
    }
};

export const concurrencyService = {
    getById: async (id: number) => {
        const response = await api.get<ConcurrencyCalculationDto>(`/${id}/get`);
        return response.data;
    },
    create: async (command: CreateConcurrencyCalculationCommand) => {
        const response = await api.put<ConcurrencyCalculationDto>('/create', command);
        return response.data;
    }
};
