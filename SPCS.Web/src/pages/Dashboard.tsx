import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { motion, AnimatePresence } from 'framer-motion';
import { Plus, Calendar, ArrowRight, Loader2, AlertCircle, LayoutDashboard, Layers } from 'lucide-react';
import { applicationService } from '../services/api';
import './Dashboard.css';

const Dashboard: React.FC = () => {
    const [isCreating, setIsCreating] = useState(false);
    const [newName, setNewName] = useState('');
    const [newDesc, setNewDesc] = useState('');
    const navigate = useNavigate();

    const queryClient = useQueryClient();

    const { data: applications, isLoading, isError } = useQuery({
        queryKey: ['applications'],
        queryFn: applicationService.getAll,
    });

    const createMutation = useMutation({
        mutationFn: applicationService.create,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['applications'] });
            setIsCreating(false);
            setNewName('');
            setNewDesc('');
        },
    });

    const handleCreate = (e: React.FormEvent) => {
        e.preventDefault();
        if (newName.trim()) {
            createMutation.mutate({ name: newName, description: newDesc });
        }
    };

    return (
        <div className="dashboard-container">
            <header className="dashboard-header">
                <div>
                    <h1>Your Applications</h1>
                    <p className="subtitle">Manage and analyze your solar battery projects</p>
                </div>
                <button className="btn btn-primary" onClick={() => setIsCreating(true)}>
                    <Plus size={20} />
                    New Application
                </button>
            </header>

            <AnimatePresence>
                {isCreating && (
                    <motion.div
                        className="modal-overlay"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                    >
                        <motion.div
                            className="glass-card modal-content"
                            initial={{ scale: 0.9, y: 20 }}
                            animate={{ scale: 1, y: 0 }}
                            exit={{ scale: 0.9, y: 20 }}
                        >
                            <h2>Create New Application</h2>
                            <form onSubmit={handleCreate}>
                                <div className="form-group">
                                    <label>Application Name</label>
                                    <input
                                        type="text"
                                        value={newName}
                                        onChange={(e) => setNewName(e.target.value)}
                                        placeholder="e.g. Residential Solar Project"
                                        required
                                    />
                                </div>
                                <div className="form-group">
                                    <label>Description (Optional)</label>
                                    <textarea
                                        value={newDesc}
                                        onChange={(e) => setNewDesc(e.target.value)}
                                        placeholder="Brief details about the project..."
                                    />
                                </div>
                                <div className="modal-actions">
                                    <button type="button" className="btn btn-outline" onClick={() => setIsCreating(false)}>Cancel</button>
                                    <button type="submit" className="btn btn-primary" disabled={createMutation.isPending}>
                                        {createMutation.isPending ? <Loader2 className="animate-spin" size={20} /> : 'Create Analysis'}
                                    </button>
                                </div>
                            </form>
                        </motion.div>
                    </motion.div>
                )}
            </AnimatePresence>

            {isLoading ? (
                <div className="loading-state">
                    <Loader2 className="spin" size={40} />
                    <p>Connecting to API...</p>
                </div>
            ) : isError ? (
                <div className="error-state">
                    <AlertCircle size={40} />
                    <h3>Connection Failed</h3>
                    <p>Could not connect to the Solar Battery API. Please ensure the backend is running at {import.meta.env.VITE_API_URL || 'https://localhost:7245/api'}.</p>
                    <button className="btn btn-ghost" onClick={() => queryClient.invalidateQueries({ queryKey: ['applications'] })}>
                        Try Again
                    </button>
                </div>
            ) : (
                <div className="app-grid">
                    {applications?.map((app, index) => (
                        <motion.div
                            key={app.id}
                            className="glass-card app-card"
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ delay: index * 0.1, duration: 0.5 }}
                            onClick={() => navigate(`/application/${app.id}`)}
                        >
                            <div className="card-inner">
                                <div className="card-top">
                                    <div className="card-icon">
                                        <LayoutDashboard size={32} />
                                    </div>
                                    <ArrowRight className="go-icon" size={24} />
                                </div>
                                <div className="card-body">
                                    <h3>{app.name}</h3>
                                    <p className="app-desc">{app.description || 'No description provided.'}</p>
                                </div>
                                <div className="card-footer">
                                    <div className="card-meta">
                                        <span><Calendar size={14} /> {new Date(app.createdAt).toLocaleDateString()}</span>
                                        <span><Layers size={14} /> {app.files?.length || 0} Files</span>
                                    </div>
                                </div>
                            </div>
                        </motion.div>
                    ))}
                    {applications?.length === 0 && (
                        <div className="empty-state">
                            <p>No applications found. Create your first one to get started!</p>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default Dashboard;
