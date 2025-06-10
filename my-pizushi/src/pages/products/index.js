import {useEffect, useRef, useState} from "react";
import {Link} from "react-router-dom";
import axiosInstance from "../../api/axiosInstance";
import {BASE_URL} from "../../api/apiConfig";
import {Card,Button,Col,Row,Spinner,Container} from "react-bootstrap";
import {Modal} from "antd";


const ProductsPage = () => {
    // const [list, setList] = useState([]);
    const [loading, setLoading] = useState(true);
    const [groupedProducts, setGroupedProducts] = useState([]);

    const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
    const [id, setDeleteId] = useState(null);

    useEffect(() => {
        axiosInstance.get("/api/Products")
            .then(res => {
                const { data } = res;
                console.log('Get list of products', data);
                // setList(data);
                groupBySlug(data);
            })
            .catch(err => console.error('Error loading products', err))
            .finally(() => setLoading(false));
    }, []);

    const groupBySlug = (items) => {
        const grouped = Object.values(items.reduce((acc, item) => {
            if (!acc[item.slug]) {
                acc[item.slug] = {
                    ...item,
                    sizes: [],
                };
            }
            acc[item.slug].sizes.push({
                sizeName: item.productSize?.name,
                price: item.price,
                id: item.id
            });
            console.log("acc",acc);
            return acc;

        }, {}));

        setGroupedProducts(grouped);
    };

    const showDeleteModal = (id) => {
        setDeleteId(id);
        setIsDeleteModalVisible(true);
    };

    const handleDeleteModalOk = async () => {
        try {
            if (!id) return;

            await axiosInstance.delete(`/api/Products/${id}`);

            setGroupedProducts(prev =>
                prev.filter(product => product.id !== id)
            );

            handleDeleteModalCancel();
            setDeleteId(null);
        } catch (error) {
            console.log("Помилка при видаленні продукту", error);
        }
    };

    const handleDeleteModalCancel = () => {
        setIsDeleteModalVisible(false);
    };

    if (loading) {
        return (
            <div className="text-center my-5">
                <Spinner animation="border" variant="primary" />
            </div>
        );
    }

    return (
        <Container className="my-4">
            <h2 className="mb-4 text-center">Продукти</h2>
            <div>
                <Link to={"create"}  className={"btn btn-success"}>Додати продукт</Link>
            </div>
            <Row xs={1} sm={2} md={3} lg={4} className="g-4">
                {groupedProducts.map(product => (
                    <Col key={product.slug}>
                        <Card className="h-100">
                            <Card.Img
                                variant="top"
                                src={`${BASE_URL}/images/800_${product.productImages?.[0]?.name}`}
                                alt={product.name}
                                style={{ objectFit: 'cover', height: '180px' }}
                            />
                            <Card.Body className="d-flex flex-column">
                                <Card.Title>{product.name}</Card.Title>

                                {product.sizes?.map((size, index) => (
                                    <div key={index} className="d-flex justify-content-between">
                                        <span>{size.sizeName}</span>
                                        <strong>{size.price} грн</strong>
                                    </div>
                                ))}
                                <br/>
                                <div className="mt-auto d-grid">
                                    <Button variant="primary">
                                        <Link to={`product/${product.id}`} className={"text-white text-decoration-none"}>Show</Link>
                                    </Button>

                                    {/*<Button className={"mt-2"} variant="success">*/}
                                        <Link to={`edit/${product.id}`} className={"btn btn-success"}>Edit</Link>
                                    {/*</Button>*/}

                                    <Button onMouseEnter={(e) => {
                                        e.target.style.backgroundColor = '#ffc107';
                                        e.target.style.color = 'white';
                                        e.target.style.borderColor = '#ffc107';
                                    }} className={"btn btn-warning text-white "} onClick={()=> showDeleteModal(product.id)}>Delete</Button>

                                </div>
                            </Card.Body>
                        </Card>
                    </Col>
                ))}
            </Row>

            <Modal
                title="Ви впевнені, що хочете видалити цей продукт?"
                open={isDeleteModalVisible}
                onOk={handleDeleteModalOk}
                onCancel={handleDeleteModalCancel}
                okText="Видалити"
                cancelText="Скасувати"
            >
            </Modal>
        </Container>
    );
};

export default ProductsPage;