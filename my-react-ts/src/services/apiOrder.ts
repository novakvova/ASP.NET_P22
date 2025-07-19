import {createApi} from "@reduxjs/toolkit/query/react";
import {createBaseQuery} from "../utilities/createBaseQuery.ts";
import {serialize} from "object-to-formdata";

export interface ICityItem {
    id: number;
    name: string;
}

export interface IPostDepartmentItem {
    id: number;
    name: string;
}

export interface IPaymentTypeItem {
    id: number;
    name: string;
}

export interface ISearchPostDepartment {
    cityId: number;
    name: string;
}

export interface ICreateOrderItem {
    recipientName: string;
    // cityId: number;
    postDepartmentId: number;
    phoneNumber: number;
    paymentTypeId: number;
}

export interface IOrder {
    id: number;
}

export const apiOrder = createApi({
    reducerPath: 'apiOrder',
    baseQuery: createBaseQuery('order'),
    tagTypes: ['Order'],
    endpoints: (builder) => ({
        getUserOrders: builder.query<IOrder, void>({
            query: () => 'list',
            providesTags: ['Order'],
        }),
        getPaymentTypes: builder.query<IPaymentTypeItem[], void>({
            query: () => 'payment-types',
            providesTags: ['Order'],
        }),
        getCities: builder.query<ICityItem[], string>({
            query: (city) => ({
                url: `search-city`,
                params: {
                    city,
                },
            }),

            providesTags: ['Order'],
        }),
        getPostDepartments: builder.query<IPostDepartmentItem[], ISearchPostDepartment>({
            query: ({ cityId, name }) => ({
                url: 'post-departments',
                params: {
                    cityId,
                    name,
                },
            }),
            providesTags: ['Order'],
        }),
        createOrder: builder.mutation<void, ICreateOrderItem>({
            query: (newOrder) => {
                const formData = serialize(newOrder);
                return {
                    url: 'create',
                    method: 'POST',
                    body: formData,
                };
            },
            invalidatesTags: ['Order'],
        })
    }),
});

export const {
    useGetPaymentTypesQuery,
    useGetCitiesQuery,
    useGetPostDepartmentsQuery,
    useCreateOrderMutation,
    useGetUserOrdersQuery,
} = apiOrder;