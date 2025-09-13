import {Badge, Button, Drawer, Form, message, Input, Select, type FormProps} from "antd";
import {useAppSelector} from "../../../store";
import React, {useState} from "react";
import {
    type ICreateOrderItem, useCreateOrderMutation,
    useGetCitiesQuery,
    useGetPaymentTypesQuery,
    useGetPostDepartmentsQuery
} from "../../../services/apiOrder.ts";
import { skipToken } from "@reduxjs/toolkit/query";
import {useGetCartQuery} from "../../../services/apiCart.ts";

interface OrderFormProps {
    onClose: () => void;
}

const OrderForm: React.FC<OrderFormProps> = ({
                                            onClose
                                             }) => {
    const [open, setOpen] = useState(false);
    const { user } = useAppSelector((state) => state.auth);
    console.log(user);
    const { refetch } = useGetCartQuery(undefined, { skip: !user });
    console.log(refetch);
    const [createOrder, {isLoading}] = useCreateOrderMutation();

    const [selectedCityId, setSelectedCityId] = useState<number | null>(null);
    const [searchDepartmentText, setSearchDepartmentText] = useState<string>("");
    const [searchCityText, setSearchCityText] = useState<string>("");

    const { data: cities = [] } = useGetCitiesQuery(searchCityText);
    const { data: paymentTypes = [] } = useGetPaymentTypesQuery();

    const {
        data: postDepartments = [],
        isLoading: isDeptsLoading
    } = useGetPostDepartmentsQuery(
        selectedCityId && searchDepartmentText
            ? { cityId: selectedCityId, name: searchDepartmentText }
            : skipToken
    );

    const onFinish: FormProps<ICreateOrderItem>['onFinish'] = async (values) => {
        try {
            await createOrder(values);
            console.log('END CREATE ORDER');
            message.success('Order created');

            refetch();
            setOpen(false);
            onClose();

        } catch (err) {
            console.error('Create order failed:', err);
        }
    };

    console.log("CITIES",cities);
    console.log("PAYMENTS",paymentTypes);
    console.log("POSTDEPARTMENTS",postDepartments);

    console.log(user);
    return (
        <>
            <Badge>
                <Button onClick={() => setOpen(true)}>Оформити замовлення</Button>
            </Badge>
            <Drawer
                title="Оформлення замовлення"
                onClose={() => setOpen(false)}
                open={open}
                width={800}
            >
                <Form
                    layout="vertical"
                    onFinish={onFinish}
                    initialValues={{
                        recipientName: user?.name || '',
                        //@ts-ignore
                        phoneNumber: user?.phone || '',
                    }}
                >
                    <Form.Item<ICreateOrderItem>
                        label="Ім’я отримувача"
                        name="recipientName"
                        rules={[{ required: true, message: 'Введіть ім’я' }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item<ICreateOrderItem>
                        label="Телефон"
                        name="phoneNumber"
                        rules={[{ required: true, message: 'Введіть номер телефону' }]}
                    >
                        <Input />
                    </Form.Item>

                    <Form.Item<ICreateOrderItem>
                        label="Місто"
                        //@ts-ignore
                        name="cityId"
                        rules={[{ required: true, message: "Оберіть місто" }]}
                    >
                        <Select
                            showSearch
                            placeholder="Оберіть місто"
                            onChange={(value) => {
                                setSelectedCityId(value);
                                setSearchDepartmentText(""); // reset department search when city changes
                            }}
                            onSearch={(value) => setSearchCityText(value)}
                            filterOption={false} // disable built-in filter to rely on server-side search
                            allowClear
                        >
                            {cities.map((city) => (
                                <Select.Option key={city.id} value={city.id}>
                                    {city.name}
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>

                    <Form.Item<ICreateOrderItem>
                        label="Відділення"
                        name="postDepartmentId"
                        rules={[{ required: true, message: "Оберіть відділення" }]}
                    >
                        <Select
                            showSearch
                            disabled={!selectedCityId}
                            placeholder={
                                selectedCityId
                                    ? "Введіть текст для пошуку відділення"
                                    : "Оберіть місто спочатку"
                            }
                            onSearch={(value) => setSearchDepartmentText(value)}
                            notFoundContent={
                                selectedCityId ? "Немає результатів" : "Оберіть місто"
                            }
                            filterOption={false}
                            loading={isDeptsLoading}
                            allowClear
                        >
                            {postDepartments.map((dept) => (
                                <Select.Option key={dept.id} value={dept.id}>
                                    {dept.name}
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>

                    <Form.Item<ICreateOrderItem>
                        label="Спосіб оплати"
                        name="paymentTypeId"
                        rules={[{ required: true, message: "Оберіть спосіб оплати" }]}
                    >
                        <Select placeholder="Оберіть спосіб оплати">
                            {paymentTypes.map((pt) => (
                                <Select.Option key={pt.id} value={pt.id}>
                                    {pt.name}
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>

                    <Form.Item>
                        <Button type="primary" htmlType="submit" loading={isLoading}>
                            Підтвердити замовлення
                        </Button>
                    </Form.Item>
                </Form>
            </Drawer>


        </>
    );
};

export default OrderForm;