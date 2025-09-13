import { useNavigate } from 'react-router-dom';
import { Form, type FormProps, Input } from 'antd';
import {type ILoginRequest, useLoginByGoogleMutation, useLoginMutation} from "../../../services/apiAccount.ts";
// import {getUserFromToken, loginSuccess} from "../../../store/authSlice.ts";
// import {useAppDispatch} from "../../../store";

import { useGoogleLogin } from '@react-oauth/google';
import LoadingOverlay from "../../../components/ui/loading/LoadingOverlay.tsx";
import {Link} from "react-router";
// import type {ServerError} from "../../../services/types.ts";



const LoginPage: React.FC = () => {
    const [login, { isLoading: isLoginLoading }] = useLoginMutation();
    const [loginByGoogle, { isLoading: isGoogleLoading }] = useLoginByGoogleMutation();

    // const dispatch = useAppDispatch();
    const navigate = useNavigate();

    // const [form] = Form.useForm<ILogin>();
    // const setServerErrors = useFormServerErrors(form);

    const onFinish: FormProps<ILoginRequest>["onFinish"] = async (values) => {
        try {
            await login(values).unwrap();
            navigate('/');
            // const { token } = response;
            // dispatch(loginSuccess(token));
            //
            // const user = getUserFromToken(token);
            // console.log("user", user);
            // if (!user || !user.roles.includes("Admin")) {
            //     navigate('/');
            // }
            // else {
            //     navigate('/admin/home');
            // }
        } catch (err) {
            console.log("error", err);
            alert("Login failed");
        }
    };

    const loginUseGoogle = useGoogleLogin({
        onSuccess: async (tokenResponse) =>
        {
            console.log("tokenResponse", tokenResponse.access_token);
            try {
                await loginByGoogle(tokenResponse.access_token).unwrap();
                // dispatch(loginSuccess(result.token));
                navigate('/');
            } catch (error) {

                console.log("User server error auth", error);
                // const serverError = error as ServerError;
                //
                // if (serverError?.status === 400 && serverError?.data?.errors) {
                //     // setServerErrors(serverError.data.errors);
                // } else {
                //     message.error("Сталася помилка при вході");
                // }
            }
        },
    });

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="bg-white p-8 rounded-2xl shadow-lg w-full max-w-md">
                {(isLoginLoading || isGoogleLoading)  && <LoadingOverlay />}

                <h2 className="text-2xl font-semibold mb-6 text-center">Admin Login</h2>
                <Form<ILoginRequest>
                    layout="vertical"
                    onFinish={onFinish}
                >
                    <Form.Item<ILoginRequest>
                        name="email"
                        label="Email"
                        rules={[{required: true, message: "Enter your email"}]}
                    >
                        <Input type="email" placeholder="you@example.com"/>
                    </Form.Item>

                    <Form.Item<ILoginRequest>
                        name="password"
                        label="Password"
                        rules={[{required: true, message: "Enter your password"}]}
                    >
                        <Input.Password placeholder="••••••••"/>
                    </Form.Item>

                    <div className="flex justify-end">
                        <Link to="/forgot-password" className="text-sm text-blue-600 hover:underline">
                            Забули пароль?
                        </Link>
                    </div>

                    <button
                        type="submit"
                        className="bg-orange-500 hover:bg-orange-600 transition text-white font-semibold px-4 py-2 rounded w-full mt-4"
                    >
                        {isLoginLoading ? 'Logging in...' : 'Login'}
                    </button>

                    <button
                        onClick={(event) => {
                            event.preventDefault();
                            loginUseGoogle();
                        }}
                        className="bg-blue-500 hover:bg-blue-600 transition text-white font-semibold px-4 py-2 rounded w-full mt-4"
                    >
                        {'LoginGoogle'}
                    </button>
                </Form>
            </div>
        </div>
    );
};

export default LoginPage;
