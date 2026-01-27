import React from 'react';
import { Sun, Battery, Settings, LayoutDashboard } from 'lucide-react';
import './Layout.css';

interface LayoutProps {
    children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
    return (
        <div className="layout-wrapper">
            <nav className="navbar">
                <div className="container nav-content">
                    <div className="logo">
                        <Sun className="logo-icon" />
                        <span>SPCS <span className="logo-accent">Solar</span></span>
                    </div>
                    <div className="nav-links">
                        <a href="/" className="nav-link active">
                            <LayoutDashboard size={18} />
                            Dashboard
                        </a>
                        <a href="/battery" className="nav-link">
                            <Battery size={18} />
                            Battery
                        </a>
                        <a href="/settings" className="nav-link">
                            <Settings size={18} />
                            Settings
                        </a>
                    </div>
                </div>
            </nav>

            <main className="main-content container">
                {children}
            </main>

            <footer className="footer">
                <div className="container">
                    <p>&copy; 2026 SPCS Solar Battery Solutions. Premium Intelligence.</p>
                </div>
            </footer>
        </div>
    );
};

export default Layout;
