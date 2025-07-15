import {createApi} from "@reduxjs/toolkit/query/react";
import {createBaseQuery} from "../utilities/createBaseQuery.ts";
import {type ICartItem} from "../store/localCartSlice.ts";


export const apiCart = createApi({
    reducerPath: 'apiCart',
    baseQuery: createBaseQuery('Cart'),
    tagTypes: ["Carts"],
    endpoints: (builder) => ({
        getCart: builder.query<ICartItem[], void>({
            query: () => ({
                url: 'getCart',
                method: 'GET'
            }),
            providesTags: ['Carts']
        }),

        addToCartsRange: builder.mutation<void, ICartItem[]>({
            query: (items) => {
                try {
                    return {
                        url: 'addRange',
                        method: 'POST',
                        body: items,
                    };
                } catch {
                    throw new Error('Error add item to cart');
                }
            },
            invalidatesTags: ["Carts"]
        }),

        createUpdateCart: builder.mutation<void, ICartItem>({
            query: (item) => {
                try {
                    return {
                        url: 'createUpdate',
                        method: 'POST',
                        body: item,
                    };
                } catch {
                    throw new Error('Error add item to cart');
                }

            },
            invalidatesTags: ["Carts"]
        }),
        removeCartItem: builder.mutation<void, number>({
            query: (id) => {
                try {
                    return {
                        url: `removeCartItem/${id}`,
                        method: 'DELETE'
                    };
                } catch {
                    throw new Error('Error remove item from cart');
                }
            },
            invalidatesTags: ["Carts"]
        }),
    })
});


export const {
    useGetCartQuery,
    useCreateUpdateCartMutation,
    useRemoveCartItemMutation
} = apiCart;