import { useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { applicationService, fileService } from '../services/api';
import { FileTypeNames } from '../types/api';
import type { ConcurrencyCalculationDto } from '../types/api';
import FileUpload from '../components/FileUpload';
import CalculationResults from '../components/CalculationResults';
import { ArrowLeft, FileText, Download, Play, AlertCircle, Loader2, Calendar, Settings2, Zap, Layers } from 'lucide-react';
import { motion, AnimatePresence } from 'framer-motion';

export default function ApplicationDetails() {
    const { id } = useParams<{ id: string }>();
    const applicationId = parseInt(id || '0');
    const queryClient = useQueryClient();

    // Calculation Form State
    const [batteryParams, setBatteryParams] = useState({
        batteryInitialState: 5,
        batteryLowestThreshold: 2,
        batteryHighestThreshold: 9,
        batteryChargingRate: 2.5,
        batteryDischargingRate: 2.5,
        batteryCapacity: 10,
    });

    const [calcResult, setCalcResult] = useState<ConcurrencyCalculationDto | null>(null);

    const { data: application, isLoading: isLoadingApp, isError: isErrorApp } = useQuery({
        queryKey: ['application', applicationId],
        queryFn: () => applicationService.getById(applicationId),
        enabled: !!applicationId,
    });

    const parseMutation = useMutation({
        mutationFn: fileService.parse,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['application', applicationId] });
        },
    });

    const calculateMutation = useMutation({
        mutationFn: (params: typeof batteryParams) =>
            applicationService.calculate(applicationId, { ...params, applicationId }),
        onSuccess: (data) => {
            setCalcResult(data);
        },
    });

    const handleParse = (fileId: number) => {
        parseMutation.mutate(fileId);
    };

    const handleDownload = async (fileId: number, fileName: string) => {
        try {
            const blob = await fileService.download(fileId);
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);
        } catch (error) {
            console.error('Download failed:', error);
        }
    };

    const handleRunCalculation = () => {
        calculateMutation.mutate(batteryParams);
    };

    if (isLoadingApp) {
        return (
            <div className="flex items-center justify-center min-h-[50vh]">
                <Loader2 className="w-10 h-10 text-blue-500 animate-spin" />
            </div>
        );
    }

    if (isErrorApp || !application) {
        return (
            <div className="flex flex-col items-center justify-center min-h-[50vh] text-center">
                <AlertCircle className="w-16 h-16 text-red-500 mb-4" />
                <h2 className="text-2xl font-bold text-white mb-2">Application Not Found</h2>
                <p className="text-gray-400 mb-6">The application you are looking for does not exist or could not be loaded.</p>
                <Link to="/" className="text-blue-400 hover:text-blue-300 flex items-center gap-2">
                    <ArrowLeft size={20} />
                    Back to Dashboard
                </Link>
            </div>
        );
    }


    return (
        <div className="container">
            <motion.div
                initial={{ opacity: 0, y: 30 }}
                animate={{ opacity: 1, y: 0 }}
                className="space-y-12 pb-32 animate-fade-in"
            >
                {/* Header Section */}
                <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
                    <div>
                        <Link to="/" className="text-dim hover:text-white flex items-center gap-2 mb-4 transition-colors font-medium text-sm">
                            <ArrowLeft size={16} />
                            Back to Analysis Overview
                        </Link>
                        <h1 className="text-5xl font-extrabold text-white tracking-tight mb-2">
                            {application.name}
                        </h1>
                        <div className="flex items-center gap-6 mt-4 text-dim text-sm font-medium">
                            <span className="flex items-center gap-2 bg-white/5 px-3 py-1.5 rounded-full border border-white/5">
                                <Calendar size={14} className="text-primary" />
                                Created {application.createdAt ? new Date(application.createdAt).toLocaleDateString() : 'N/A'}
                            </span>
                            <span className="flex items-center gap-2 bg-white/5 px-3 py-1.5 rounded-full border border-white/5">
                                <Layers size={14} className="text-accent" />
                                {application.files?.length || 0} Data Source Files
                            </span>
                        </div>
                    </div>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-12 gap-10">
                    {/* Main Content Area */}
                    <div className="lg:col-span-8 space-y-10">
                        {/* Results Section */}
                        <AnimatePresence>
                            {calcResult && (
                                <motion.div
                                    initial={{ opacity: 0, height: 0 }}
                                    animate={{ opacity: 1, height: 'auto' }}
                                    exit={{ opacity: 0, height: 0 }}
                                    className="overflow-hidden"
                                >
                                    <div className="mb-10">
                                        <div className="flex items-center justify-between mb-8">
                                            <h2 className="text-2xl font-bold text-white flex items-center gap-3">
                                                <div className="p-2 bg-yellow-500/10 rounded-lg">
                                                    <Zap size={24} className="text-yellow-400" />
                                                </div>
                                                Intelligence Report
                                            </h2>
                                            <button
                                                onClick={() => setCalcResult(null)}
                                                className="text-xs text-dim hover:text-white transition-colors"
                                            >
                                                Dismiss Report
                                            </button>
                                        </div>
                                        <CalculationResults data={calcResult} />
                                    </div>
                                    <div className="h-px bg-gradient-to-r from-transparent via-white/10 to-transparent mb-12" />
                                </motion.div>
                            )}
                        </AnimatePresence>

                        {/* Files Management Area */}
                        <div className="glass-card shadow-2xl min-h-[500px] border-white/5 border-t-white/10 p-10">
                            <div className="flex items-center justify-between mb-10">
                                <h2 className="text-2xl font-bold text-white flex items-center gap-3">
                                    <div className="p-2 bg-blue-500/10 rounded-lg">
                                        <FileText size={24} className="text-blue-400" />
                                    </div>
                                    System Data Sources
                                </h2>
                            </div>

                            {application.files && application.files.length > 0 ? (
                                <div className="grid gap-6">
                                    {application.files.map((file) => (
                                        <motion.div
                                            key={file.id}
                                            whileHover={{ x: 4 }}
                                            className="group flex flex-col md:flex-row md:items-center justify-between p-6 bg-white/[0.02] rounded-2xl border border-white/5 hover:border-white/10 hover:bg-white/[0.04] transition-all gap-6"
                                        >
                                            <div className="flex items-center gap-6">
                                                <div className="w-14 h-14 flex items-center justify-center bg-blue-500/10 rounded-2xl group-hover:bg-blue-500/20 transition-colors">
                                                    <FileText size={26} className="text-blue-400" />
                                                </div>
                                                <div>
                                                    <p className="font-bold text-lg text-white group-hover:text-blue-200 transition-colors mb-1">{file.name}</p>
                                                    <div className="flex items-center gap-3 text-xs font-semibold text-dim uppercase tracking-wider">
                                                        <span className={`px-2.5 py-1 rounded-md ${file.type === 1 ? 'bg-orange-500/10 text-orange-400' :
                                                            file.type === 2 ? 'bg-green-500/10 text-green-400' :
                                                                'bg-blue-500/10 text-blue-400'
                                                            }`}>
                                                            {FileTypeNames[file.type as keyof typeof FileTypeNames]}
                                                        </span>
                                                        <span className="opacity-30">•</span>
                                                        <span>{(file.path || '').split('.').pop()?.toUpperCase() || 'RAW'} DATA</span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div className="flex items-center gap-3">
                                                <button
                                                    onClick={() => handleParse(file.id)}
                                                    className="btn btn-outline border-green-500/20 text-green-400 hover:bg-green-500/10 hover:border-green-500 px-6 py-2.5 text-xs"
                                                    disabled={parseMutation.isPending}
                                                >
                                                    {parseMutation.isPending ? <Loader2 size={16} className="animate-spin" /> : <Play size={16} />}
                                                    Parse Dataset
                                                </button>
                                                <button
                                                    onClick={() => handleDownload(file.id, file.name)}
                                                    className="btn btn-outline px-4 py-2.5 text-dim hover:text-white"
                                                    title="Download Raw Data"
                                                >
                                                    <Download size={18} />
                                                </button>
                                            </div>
                                        </motion.div>
                                    ))}
                                </div>
                            ) : (
                                <div className="flex flex-col items-center justify-center py-24 text-dim border border-dashed border-white/5 rounded-3xl">
                                    <div className="p-8 bg-white/[0.02] rounded-full mb-8">
                                        <FileText size={72} className="opacity-10" />
                                    </div>
                                    <p className="font-bold text-lg mb-2 text-white/40">No system data files linked yet.</p>
                                    <p className="text-sm opacity-60">Upload files to begin the intelligence analysis.</p>
                                </div>
                            )}
                        </div>
                    </div>

                    {/* Sidebar - Upload & Calculations */}
                    <div className="space-y-6">
                        <FileUpload
                            applicationId={applicationId}
                            onUploadSuccess={() => queryClient.invalidateQueries({ queryKey: ['application', applicationId] })}
                        />

                        {/* Concurrency Calculation Form */}
                        <div className="bg-white/5 backdrop-blur-md rounded-xl border border-white/10 p-6">
                            <h3 className="text-xl font-semibold text-white mb-4 flex items-center gap-2">
                                <Settings2 size={20} className="text-blue-400" />
                                Calculation Settings
                            </h3>

                            <div className="space-y-3">
                                <div>
                                    <label className="text-xs text-gray-400 block mb-1">Battery Capacity (kWh)</label>
                                    <input
                                        type="number"
                                        className="w-full bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none"
                                        value={batteryParams.batteryCapacity}
                                        onChange={(e) => setBatteryParams({ ...batteryParams, batteryCapacity: parseFloat(e.target.value) })}
                                    />
                                </div>
                                <div className="grid grid-cols-2 gap-3">
                                    <div>
                                        <label className="text-xs text-gray-400 block mb-1">Initial (kWh)</label>
                                        <input
                                            type="number"
                                            className="w-full bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none"
                                            value={batteryParams.batteryInitialState}
                                            onChange={(e) => setBatteryParams({ ...batteryParams, batteryInitialState: parseFloat(e.target.value) })}
                                        />
                                    </div>
                                    <div>
                                        <label className="text-xs text-gray-400 block mb-1">Low (kWh)</label>
                                        <input
                                            type="number"
                                            className="w-full bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none"
                                            value={batteryParams.batteryLowestThreshold}
                                            onChange={(e) => setBatteryParams({ ...batteryParams, batteryLowestThreshold: parseFloat(e.target.value) })}
                                        />
                                    </div>
                                </div>
                                <div className="grid grid-cols-2 gap-3">
                                    <div>
                                        <label className="text-xs text-gray-400 block mb-1">Charge (kW)</label>
                                        <input
                                            type="number"
                                            className="w-full bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none"
                                            value={batteryParams.batteryChargingRate}
                                            onChange={(e) => setBatteryParams({ ...batteryParams, batteryChargingRate: parseFloat(e.target.value) })}
                                        />
                                    </div>
                                    <div>
                                        <label className="text-xs text-gray-400 block mb-1">Discharge (kW)</label>
                                        <input
                                            type="number"
                                            className="w-full bg-white/5 border border-white/10 rounded-lg px-3 py-2 text-white text-sm focus:border-blue-500 outline-none"
                                            value={batteryParams.batteryDischargingRate}
                                            onChange={(e) => setBatteryParams({ ...batteryParams, batteryDischargingRate: parseFloat(e.target.value) })}
                                        />
                                    </div>
                                </div>

                                <button
                                    onClick={handleRunCalculation}
                                    disabled={calculateMutation.isPending || !application.files?.length}
                                    className="btn btn-primary w-full mt-4"
                                >
                                    {calculateMutation.isPending ? <Loader2 size={18} className="animate-spin" /> : <Play size={18} />}
                                    Run Calculation
                                </button>

                                {calculateMutation.isError && (
                                    <p className="text-xs text-red-400 text-center mt-2 flex items-center justify-center gap-1">
                                        <AlertCircle size={12} />
                                        Error: {(calculateMutation.error as any).response?.data?.Message || 'Calculation failed'}
                                    </p>
                                )}
                            </div>
                        </div>
                    </div>
                </div>
            </motion.div>
        </div>
    );
}
