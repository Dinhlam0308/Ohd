import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "./components/login/Login.jsx";
import ForgotPassword from "./components/forgotpassword/ForgotPassword.jsx";
export default function App() {
    return (
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />
                {/*<Route*/}
                {/*    path="/change-password-first-login"*/}
                {/*    element={<ChangePasswordFirstLoginPage />}*/}
                {/*/>*/}
                
                {/*/!* TODO: admin/technician routes ở đây *!/*/}
                
                {/*/!* default *!/*/}
                {/*<Route path="*" element={<Navigate to="/login" replace />} />*/}
            </Routes>
    );
}