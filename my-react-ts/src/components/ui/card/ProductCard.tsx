import {Card, Col, Tooltip, Image} from 'antd';
import {APP_ENV} from "../../../env";
import {useAppSelector} from "../../../store";
import type {ICartItem} from "../../../store/localCartSlice.ts";
import {useCart} from "../../../hooks/useCart.ts";


interface Ingredient {
    id: number;
    name: string;
    image: string;
}

interface ProductCardProps {
    product: {
        id: number;
        name: string;
        slug: string;
        price: number;
        weight: number;
        productSize?: { name: string };
        ingredients?: Ingredient[];
        productImages?: { name: string }[];
    };
}



export const ProductCard: React.FC<ProductCardProps> = ({product}) => {
    const mainImage = product.productImages?.[0]?.name;
    const ingredients = product.ingredients || [];
    const visible = ingredients.slice(0, 2);
    const hidden = ingredients.slice(2);
    const {user} = useAppSelector(state => state.auth);

    const { cart, addToCart } = useCart(user!=null);

    const isInCart = cart.some(item =>
        product && item.productId ===  product.id
    );

    // console.log("cart", cart);
    // console.log("product", product);
    // console.log("isInCart", isInCart);
    const handleAddToCart = async () => {
        if (!product) return;

        console.log("product add", product);

        const newItem: ICartItem = {
            id: product.id,
            productId: product.id,
            quantity: 1,
            sizeName: product.productSize?.name ?? "",
            price: product.price,
            imageName: product.productImages?.[0]?.name ?? "",
            categoryId: 0,
            categoryName: "",
            name: product.name,
        };

        await addToCart(newItem);

    };

    return (
        <Col xs={24} sm={12} md={8} lg={6}>
            <div className="h-full flex">
                <Card
                    hoverable
                    className="w-full flex flex-col"
                    cover={
                        mainImage ? (
                            <img
                                alt={product.name}
                                src={`${APP_ENV.IMAGES_400_URL}${mainImage}`}
                                className="h-[200px] object-cover"
                            />
                        ) : null
                    }
                    title={product.name}
                >
                    <div className="flex flex-col justify-between flex-1">
                        <div>
                            <p><strong>Ціна:</strong> {product.price} грн</p>
                            <p><strong>Вага:</strong> {product.weight} г</p>

                            {product.productSize && (
                                <p><strong>Розмір:</strong> {product.productSize.name}</p>
                            )}

                            {ingredients.length > 0 && (
                                <div>
                                    <strong>Інгредієнти:</strong>
                                    <div className="flex flex-wrap gap-2 mt-2">
                                        {visible.map((ingredient) => (
                                            <Tooltip title={ingredient.name} key={ingredient.id}>
                                                <Image
                                                    src={`${APP_ENV.IMAGES_400_URL}${ingredient.image}`}
                                                    alt={ingredient.name}
                                                    width={40}
                                                    height={40}
                                                    className="rounded-full"
                                                    preview={false}
                                                />
                                            </Tooltip>
                                        ))}

                                        {hidden.length > 0 && (
                                            <Tooltip
                                                title={
                                                    <div className="flex flex-wrap gap-2">
                                                        {hidden.map((ingredient) => (
                                                            <Tooltip title={ingredient.name} key={ingredient.id}>
                                                                <Image
                                                                    src={`${APP_ENV.IMAGES_400_URL}${ingredient.image}`}
                                                                    alt={ingredient.name}
                                                                    width={40}
                                                                    height={40}
                                                                    className="rounded-full"
                                                                    preview={false}
                                                                />
                                                            </Tooltip>
                                                        ))}
                                                    </div>
                                                }
                                            >
                                                <div
                                                    className="w-10 h-10 rounded-full bg-gray-200 flex items-center justify-center font-semibold cursor-pointer text-sm leading-none shrink-0 self-center"
                                                >
                                                    +{hidden.length}
                                                </div>
                                            </Tooltip>
                                        )}
                                    </div>
                                </div>
                            )}

                            <button className={`${
                                isInCart ? "bg-gray-400 cursor-not-allowed" : "bg-green-500 hover:bg-green-700"
                            } text-white font-bold py-2 px-4 mt-5 rounded-full`}
                                    onClick={!isInCart ? handleAddToCart : undefined}
                            >
                                {isInCart ? "Вже в кошику" : "В кошик"}
                            </button>
                        </div>
                    </div>
                </Card>
            </div>
        </Col>
    );
};

export default ProductCard;