import React, { useState } from 'react';
import { InputText } from 'primereact/inputtext';
import { FloatLabel } from "primereact/floatlabel";

import { Password } from 'primereact/password';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import { classNames } from 'primereact/utils';
import { Link } from 'react-router-dom';

const Login = () => {
    const [username, setUsername] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [submitted, setSubmitted] = useState<boolean>(false);
    const toast = React.useRef(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        setSubmitted(true);

        if (username && password) {
            // Handle the login logic here
            toast.current.show({ severity: 'success', summary: 'Login Successful', detail: 'Welcome back!' });
        } else {
            toast.current.show({ severity: 'error', summary: 'Login Failed', detail: 'Please check your credentials' });
        }
    }

    const getFormErrorMessage = (name) => {
        return submitted && !name ? <small className="p-error">This field is required.</small> : null;
    }

    return (
        <div className="p-d-flex p-jc-center p-ai-center p-min-vh-100 p-p-4">
            <Toast ref={toast} />
            <div className="card p-shadow-3 p-p-4" style={{ width: '25rem' }}>
                <h2 className="p-text-center">Login</h2>
                <form onSubmit={handleSubmit} className="p-fluid">
                    <div className="p-field">
                        <span className="p-input-icon-left">
                            <FloatLabel>
                                <InputText 
                                    id="username" 
                                    value={username} 
                                    onChange={(e) => setUsername(e.target.value)} 
                                    className={classNames({ 'p-invalid': submitted && !username })}
                                    placeholder="Enter your username"
                                />
                                <label htmlFor="username">Username</label>
                            </FloatLabel>
                            
                        </span>
                        {getFormErrorMessage(username)}
                    </div>
                    <div className="p-field">
                        <span className="p-input-icon-left">
                            <i className="pi pi-lock" />
                            <FloatLabel>
                                <Password 
                                    id="password" 
                                    value={password} 
                                    onChange={(e) => setPassword(e.target.value)} 
                                    feedback={false} 
                                    className={classNames({ 'p-invalid': submitted && !password })}
                                    placeholder="Enter your password"
                                />
                                <label htmlFor="password">Password</label>
                            </FloatLabel>
                        </span>
                        {getFormErrorMessage(password)}
                    </div>
                    <Button label="Login" type="submit" className="p-mt-2" />
                </form>
                <div className="p-d-flex p-jc-between p-mt-3">
                    <Link to="/" className="p-text-secondary p-mr-2">Go Back</Link>
                    <Link to="/signup" className="p-text-secondary">Don't have an account? Sign Up</Link>
                </div>
            </div>
        </div>
    );
}

export default Login;
