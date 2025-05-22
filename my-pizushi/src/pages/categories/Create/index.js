import {useState} from "react";
import axiosInstance from "../../../api/axiosInstance";
import {BASE_URL} from "../../../api/apiConfig";
import {useNavigate} from "react-router-dom";
import BaseTextInput from "../../../components/common/BaseTextInput";
import BaseFileInput from "../../../components/common/BaseFileInput";

import * as Yup from "yup";
import {useFormik} from "formik";

const validationSchema = Yup.object().shape({
    name: Yup.string().required("Вкажіть назву"),
    slug: Yup.string().required("Вкажіть слаг"),
    imageFile: Yup.mixed().nullable()
});

const CategoriesCreatePage = () => {

    const initValues = {
        name: "",
        slug: "",
        imageFile: null,
    };

    const handleFormikSubmit = async (values) => {
        console.log("Submit formik", values);
        try {
            var result = await axiosInstance.post(`${BASE_URL}/api/categories`, values,
                {
                    headers: {
                        "Content-Type": "multipart/form-data"
                    }
                });
            console.log("Server result", result);
            navigate("..");

        } catch(err) {
            //Що з цим робити і як робити на сервері
            console.error("Send request error", err);

            const serverErrors = {};
            const {response} = err;
            const {data} = response;
            if(data) {
                const {errors} = data;
                Object.entries(errors).forEach(([key, messages]) => {
                    let messageLines = "";
                    messages.forEach(message => {
                        messageLines += message+" ";
                        console.log(`${key}: ${message}`);
                    });
                    const field = key.charAt(0).toLowerCase() + key.slice(1);
                    serverErrors[field] = messageLines;

                });
            }
            console.log("response", response);
            console.log("serverErrors", serverErrors);
            setErrors(serverErrors);

        }
    }

    const formik = useFormik({
        initialValues: initValues,
        onSubmit: handleFormikSubmit,
        validationSchema: validationSchema,
    });

    const {values, handleSubmit, errors, touched, setErrors, handleChange, setFieldValue} = formik;

    const navigate = useNavigate();

    const onHandleFileChange = (e) => {
        const files = e.target.files;
        if (files.length > 0) {
            setFieldValue("imageFile", files[0]);
        }
        else {
            setFieldValue("imageFile", null);
        }
    }

    return (
        <>
            <h1 className={"text-center"}>Додати категорію</h1>
            <form onSubmit={handleSubmit} className={"col-md-6 offset-md-3"}>
                <BaseTextInput
                    label={"Назва"}
                    field={"name"}
                    error={errors.name}
                    touched={touched.name}
                    value={values.name}
                    onChange={handleChange}
                />

                <BaseTextInput
                    label={"Url-Slug"}
                    field={"slug"}
                    error={errors.slug}
                    touched={touched.slug}
                    value={values.slug}
                    onChange={handleChange}
                />

                <BaseFileInput
                    label={"Оберіть фото"}
                    field={"imageFile"}
                    error={errors.imageFile}
                    touched={touched.imageFile}
                    onChange={onHandleFileChange}
                />

                <button type="submit" className="btn btn-primary">Додати</button>
            </form>
        </>
    )
}

export default CategoriesCreatePage