import { useState, useRef } from 'react';
import { Upload, CheckCircle, AlertCircle, Loader2 } from 'lucide-react';
import { fileService } from '../services/api';
import { FileTypeNames } from '../types/api';
import type { FileType } from '../types/api';
import { motion, AnimatePresence } from 'framer-motion';

interface FileUploadProps {
    applicationId: number;
    onUploadSuccess: () => void;
}

export default function FileUpload({ applicationId, onUploadSuccess }: FileUploadProps) {
    const [isDragging, setIsDragging] = useState(false);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [selectedType, setSelectedType] = useState<FileType>(1); // Default to Solar Data
    const [uploading, setUploading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);
    const fileInputRef = useRef<HTMLInputElement>(null);

    const handleDragOver = (e: React.DragEvent) => {
        e.preventDefault();
        setIsDragging(true);
    };

    const handleDragLeave = (e: React.DragEvent) => {
        e.preventDefault();
        setIsDragging(false);
    };

    const handleDrop = (e: React.DragEvent) => {
        e.preventDefault();
        setIsDragging(false);
        const files = e.dataTransfer.files;
        if (files.length > 0) {
            validateAndSetFile(files[0]);
        }
    };

    const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            validateAndSetFile(e.target.files[0]);
        }
    };

    const validateAndSetFile = (file: File) => {
        setError(null);
        setSuccess(false);
        // Add file validation logic here if needed (e.g., size, type)
        setSelectedFile(file);
    };

    const handleUpload = async () => {
        if (!selectedFile) return;

        setUploading(true);
        setError(null);

        try {
            await fileService.upload(selectedFile, selectedType, applicationId);
            setSuccess(true);
            setSelectedFile(null);
            if (fileInputRef.current) fileInputRef.current.value = '';
            onUploadSuccess();

            // Reset success message after 3 seconds
            setTimeout(() => setSuccess(false), 3000);
        } catch (err) {
            console.error('Upload failed:', err);
            setError('Failed to upload file. Please try again.');
        } finally {
            setUploading(false);
        }
    };

    return (
        <div className="glass-card border-white/5 border-t-white/10 p-8">
            <h3 className="text-xl font-bold text-white mb-8 flex items-center gap-3">
                <div className="p-2 bg-primary/10 rounded-lg">
                    <Upload size={22} className="text-primary" />
                </div>
                Ingest Dataset
            </h3>

            <div className="space-y-6">
                {/* File Type Selection */}
                <div className="grid grid-cols-3 gap-3">
                    {(Object.entries(FileTypeNames) as [string, string][]).map(([key, label]) => {
                        const typeValue = parseInt(key) as FileType;
                        if (typeValue === 0) return null; // Skip Unknown

                        return (
                            <button
                                key={key}
                                onClick={() => setSelectedType(typeValue)}
                                className={`px-2 py-2.5 rounded-xl text-xs font-bold uppercase tracking-wider transition-all duration-300 border ${selectedType === typeValue
                                    ? 'bg-primary border-primary text-white shadow-[0_8px_20px_-6_rgba(59,130,246,0.5)]'
                                    : 'bg-white/5 border-white/5 text-dim hover:bg-white/10 hover:text-white'
                                    }`}
                            >
                                {label.split(' ')[0]}
                            </button>
                        );
                    })}
                </div>

                {/* Drop Zone */}
                <div
                    onDragOver={handleDragOver}
                    onDragLeave={handleDragLeave}
                    onDrop={handleDrop}
                    onClick={() => fileInputRef.current?.click()}
                    className={`relative border-2 border-dashed rounded-3xl p-16 text-center cursor-pointer transition-all duration-500 group ${isDragging
                        ? 'border-primary bg-primary/5 scale-[1.02]'
                        : success
                            ? 'border-success bg-success/5'
                            : 'border-white/5 hover:border-white/20 hover:bg-white/[0.02]'
                        }`}
                >
                    <input
                        type="file"
                        ref={fileInputRef}
                        onChange={handleFileSelect}
                        style={{ display: 'none' }}
                        tabIndex={-1}
                    />

                    <AnimatePresence mode="wait">
                        {uploading ? (
                            <motion.div
                                key="uploading"
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                exit={{ opacity: 0, scale: 0.9 }}
                                className="flex flex-col items-center gap-6"
                            >
                                <Loader2 className="w-14 h-14 text-primary animate-spin" />
                                <p className="text-primary font-bold tracking-widest uppercase text-xs">Encryption in progress...</p>
                            </motion.div>
                        ) : success ? (
                            <motion.div
                                key="success"
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                exit={{ opacity: 0, scale: 0.9 }}
                                className="flex flex-col items-center gap-6"
                            >
                                <div className="p-5 bg-success/10 rounded-2xl ring-1 ring-success/20">
                                    <CheckCircle className="w-12 h-12 text-success" />
                                </div>
                                <p className="text-success font-bold tracking-widest uppercase text-xs text-lg">Transmission Success</p>
                            </motion.div>
                        ) : selectedFile ? (
                            <motion.div
                                key="selected"
                                initial={{ opacity: 0, y: 10 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -10 }}
                                className="flex flex-col items-center gap-6"
                            >
                                <div className="p-5 bg-primary/10 rounded-2xl ring-1 ring-primary/20 group-hover:bg-primary/20 transition-colors">
                                    <Upload className="w-12 h-12 text-primary" />
                                </div>
                                <div>
                                    <p className="text-white font-bold text-lg">{selectedFile.name}</p>
                                    <p className="text-xs text-dim font-bold uppercase tracking-widest mt-2">{(selectedFile.size / 1024).toFixed(1)} KB READY</p>
                                </div>
                                <div className="flex flex-col gap-3 w-full mt-6">
                                    <button
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            handleUpload();
                                        }}
                                        className="btn btn-primary w-full py-4"
                                    >
                                        Execute Upload
                                    </button>
                                    <button
                                        onClick={(e) => {
                                            e.stopPropagation();
                                            setSelectedFile(null);
                                            if (fileInputRef.current) fileInputRef.current.value = '';
                                        }}
                                        className="btn btn-outline w-full py-3"
                                    >
                                        Cancel
                                    </button>
                                </div>
                            </motion.div>
                        ) : (
                            <motion.div
                                key="empty"
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                exit={{ opacity: 0 }}
                                className="flex flex-col items-center gap-6"
                            >
                                <div className="p-6 bg-white/[0.02] rounded-3xl border border-white/5 group-hover:scale-110 group-hover:border-primary/20 transition-all duration-500">
                                    <Upload className="w-12 h-12 text-dim group-hover:text-primary transition-colors" />
                                </div>
                                <div>
                                    <p className="text-white font-bold text-lg group-hover:text-primary transition-colors">Select Dataset</p>
                                    <p className="text-sm text-dim font-medium mt-3">Drag and drop CSV or JSON logs here</p>
                                </div>
                            </motion.div>
                        )}
                    </AnimatePresence>
                </div>

                {error && (
                    <motion.div
                        initial={{ opacity: 0, x: -10 }}
                        animate={{ opacity: 1, x: 0 }}
                        className="flex items-center gap-3 p-4 bg-red-500/10 border border-red-500/20 rounded-xl text-red-400 text-xs font-bold tracking-tight"
                    >
                        <AlertCircle size={18} />
                        {error}
                    </motion.div>
                )}
            </div>
        </div>
    );
}
