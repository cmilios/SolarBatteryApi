import type { ConcurrencyCalculationDto } from '../types/api';
import {
    LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer
} from 'recharts';
import { motion } from 'framer-motion';
import { Zap, Battery, ArrowDownLeft, ArrowUpRight } from 'lucide-react';

interface CalculationResultsProps {
    data: ConcurrencyCalculationDto;
}

export default function CalculationResults({ data }: CalculationResultsProps) {
    // Merge data for the main chart
    const chartData = data.batteryHistory.map((item, index) => ({
        time: new Date(item.date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }),
        battery: item.value,
        toNetwork: data.powerToTheNetwork[index]?.value || 0,
        fromNetwork: data.powerFromTheNetwork[index]?.value || 0,
    }));

    const MetricCard = ({ title, value, unit, icon: Icon, colorClass }: any) => (
        <div className="glass-card flex items-center gap-5 border-white/5 group hover:border-primary/20 transition-all">
            <div className={`p-4 rounded-2xl ${colorClass} transition-transform group-hover:scale-110 duration-300`}>
                <Icon size={28} />
            </div>
            <div>
                <p className="text-xs font-bold text-dim uppercase tracking-wider mb-1">{title}</p>
                <div className="flex items-baseline gap-1">
                    <span className="text-3xl font-extrabold text-white tracking-tighter">
                        {typeof value === 'number' ? value.toFixed(1) : value}
                    </span>
                    <span className="text-xs font-semibold text-dim">{unit}</span>
                </div>
            </div>
        </div>
    );

    return (
        <motion.div
            initial={{ opacity: 0, scale: 0.98 }}
            animate={{ opacity: 1, scale: 1 }}
            className="space-y-8"
        >
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <MetricCard
                    title="Efficiency"
                    value={data.concurrencyMetric * 100}
                    unit="%"
                    icon={Zap}
                    colorClass="bg-yellow-500/10 text-yellow-400 border border-yellow-500/20"
                />
                <MetricCard
                    title="Coverage"
                    value={data.needCoverage * 100}
                    unit="%"
                    icon={Battery}
                    colorClass="bg-success/10 text-success border border-success/20"
                />
                <MetricCard
                    title="Exported"
                    value={data.powerToTheNetwork.reduce((acc, curr) => acc + curr.value, 0)}
                    unit="kWh"
                    icon={ArrowUpRight}
                    colorClass="bg-blue-500/10 text-blue-400 border border-blue-500/20"
                />
                <MetricCard
                    title="Imported"
                    value={data.powerFromTheNetwork.reduce((acc, curr) => acc + curr.value, 0)}
                    unit="kWh"
                    icon={ArrowDownLeft}
                    colorClass="bg-red-500/10 text-red-400 border border-red-500/20"
                />
            </div>

            <div className="glass-card p-8 border-white/5 border-t-white/10">
                <div className="flex items-center justify-between mb-10">
                    <h3 className="text-xl font-bold text-white flex items-center gap-3">
                        <div className="w-2 h-8 bg-primary rounded-full" />
                        Energy Flow Dynamics
                    </h3>
                    <div className="flex gap-4">
                        {/* Legend Simulation */}
                        <div className="flex items-center gap-2 text-xs font-semibold text-dim">
                            <span className="w-3 h-3 rounded-full bg-primary shadow-[0_0_8px_rgba(59,130,246,0.5)]" /> Battery
                        </div>
                        <div className="flex items-center gap-2 text-xs font-semibold text-dim">
                            <span className="w-3 h-3 rounded-full bg-accent shadow-[0_0_8px_rgba(251,191,36,0.5)]" /> Export
                        </div>
                        <div className="flex items-center gap-2 text-xs font-semibold text-dim">
                            <span className="w-3 h-3 rounded-full bg-danger shadow-[0_0_8px_rgba(239,68,68,0.5)]" /> Import
                        </div>
                    </div>
                </div>

                <div className="h-[450px] w-full">
                    <ResponsiveContainer width="100%" height="100%">
                        <LineChart data={chartData} margin={{ top: 5, right: 30, left: 0, bottom: 5 }}>
                            <CartesianGrid strokeDasharray="3 3" stroke="rgba(255,255,255,0.03)" vertical={false} />
                            <XAxis
                                dataKey="time"
                                stroke="#64748b"
                                fontSize={11}
                                fontWeight={600}
                                tickLine={false}
                                axisLine={false}
                                dy={15}
                            />
                            <YAxis
                                stroke="#64748b"
                                fontSize={11}
                                fontWeight={600}
                                tickLine={false}
                                axisLine={false}
                                tickFormatter={(value) => `${value}kW`}
                                dx={-10}
                            />
                            <Tooltip
                                contentStyle={{
                                    backgroundColor: 'rgba(15, 23, 42, 0.95)',
                                    borderColor: 'rgba(255, 255, 255, 0.1)',
                                    borderRadius: '16px',
                                    backdropFilter: 'blur(10px)',
                                    boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.5)',
                                    border: '1px solid rgba(255,255,255,0.08)',
                                    padding: '12px'
                                }}
                                itemStyle={{ fontSize: '12px', fontWeight: 'bold' }}
                                cursor={{ stroke: 'rgba(255,255,255,0.1)', strokeWidth: 1 }}
                            />
                            <Line
                                type="monotone"
                                dataKey="battery"
                                stroke="var(--color-primary)"
                                strokeWidth={4}
                                dot={false}
                                name="Battery Level"
                                animationDuration={2000}
                            />
                            <Line
                                type="monotone"
                                dataKey="toNetwork"
                                stroke="var(--color-accent)"
                                strokeWidth={2}
                                strokeDasharray="5 5"
                                dot={false}
                                name="Exporting"
                                animationDuration={2500}
                            />
                            <Line
                                type="monotone"
                                dataKey="fromNetwork"
                                stroke="var(--color-danger)"
                                strokeWidth={2}
                                strokeDasharray="5 5"
                                dot={false}
                                name="Importing"
                                animationDuration={3000}
                            />
                        </LineChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </motion.div>
    );
}
