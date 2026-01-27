export interface ApplicationDto {
    id: number;
    name: string;
    description?: string;
    createdAt: string;
    files?: FileDto[];
}

export interface CreateApplicationCommand {
    name: string;
    description?: string;
}

export type FileType = 0 | 1 | 2 | 3;

export const FileTypeNames = {
    0: 'Unknown',
    1: 'Solar Data',
    2: 'Battery Data',
    3: 'Consumption Data'
} as const;

export interface FileDto {
    id: number;
    name: string;
    contentType: string;
    type: FileType;
    applicationId: number;
    path?: string;
}

export interface PowerTimestampDto {
    id: number;
    date: string;
    value: number;
    type: number;
}

export interface ConcurrencyCalculationDto {
    id: number;
    batteryHistory: PowerTimestampDto[];
    powerToTheNetwork: PowerTimestampDto[];
    powerFromTheNetwork: PowerTimestampDto[];
    concurrencyMetric: number;
    needCoverage: number;
}

export interface CreateConcurrencyCalculationCommand {
    applicationId: number;
    batteryInitialState: number;
    batteryLowestThreshold: number;
    batteryHighestThreshold: number;
    batteryChargingRate: number;
    batteryDischargingRate: number;
    batteryCapacity: number;
}
