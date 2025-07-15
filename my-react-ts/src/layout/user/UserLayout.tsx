import {Link, Outlet} from "react-router";
import {useAppDispatch, useAppSelector} from "../../store";
// import {useNavigate} from "react-router-dom";
import {logout} from "../../store/authSlice.ts";
import {Button} from "antd";
import {APP_ENV} from "../../env";
import {useCart} from "../../hooks/useCart.ts";
import {apiCart} from "../../services/apiCart.ts";
import {addItem} from "../../store/localCartSlice.ts";
import CartDrawer from "../../components/Cart/CartDrewer";

const UserLayout: React.FC = () => {
    const {user} = useAppSelector(state => state.auth);

    const { cart } = useCart(user!=null);

    const dispatch = useAppDispatch();

    // console.log("items", items);
    const logoutHandler = async () => {
        // if (!serverCart?.items) return;

        const serverCart = [...cart];
        dispatch(logout());
        console.log('Server cart', serverCart);
        dispatch(apiCart.util.resetApiState()); // очищення кешу запитів кошика
        console.log('Server cart', serverCart);
        serverCart.forEach(item => {
            dispatch(addItem(item));
        });
    }


    return (
        <div className="min-h-screen flex flex-col bg-white text-gray-900 dark:bg-gray-900 dark:text-white">
            <header className="w-full py-4 px-6 bg-orange-500 text-white shadow-md flex justify-between">
                <h1 className="text-xl font-semibold">FoodDelivery</h1>

                <div className="flex items-center gap-4">
                    <CartDrawer />
                    {user ? (
                        <>
                            <Link to="/account" className="flex items-center gap-2">
                                <img
                                    src={user.image ? `${APP_ENV.IMAGES_50_URL}${user.image}` : '/images/user/default.png'}
                                    alt={user.name}
                                    className="w-10 h-10 rounded-full border-2 border-white object-cover"
                                />
                                <span className="font-medium">{user.name}</span>
                            </Link>

                            <Link
                                to="/admin/home"
                                className="bg-white text-orange-500 px-3 py-1 rounded hover:bg-orange-100 transition"
                            >
                                Адмінка
                            </Link>


                            <Button
                                onClick={() => logoutHandler()}
                                className="bg-white text-orange-500 border-none hover:bg-orange-100"
                            >
                                Вихід
                            </Button>
                        </>
                    ) : (
                        <>
                            <Link
                                to="login"
                                className="bg-white text-orange-500 px-4 py-2 rounded hover:bg-orange-100 transition"
                            >
                                Вхід
                            </Link>

                            <Link
                                to="register"
                                className="bg-white text-orange-500 px-4 py-2 rounded hover:bg-orange-100 transition"
                            >
                                Реєстрація
                            </Link>
                        </>
                    )}
                </div>

            </header>

            <main className="flex-1 p-4 md:p-6">
                <Outlet/>
            </main>

            <footer className="w-full py-3 px-6 bg-gray-100 text-sm text-center dark:bg-gray-800 dark:text-gray-300">
                © 2025 FoodDelivery. Усі права захищено.
            </footer>
        </div>
    );
};

export default UserLayout;
