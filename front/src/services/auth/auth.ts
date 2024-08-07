import axios from "axios"
import { url } from "../../constants/constants"
import { User } from "../../Models/user.model";
import { DataResponse } from "../../Models/data-response";


export const CreateUser = async (email: string, password: string, role: string, companyName: string, date: Date) => {

    const token = localStorage.getItem("token"); //whatever the devil we call it.

    if (!token || token == "") {
        const failedTokenResponse: DataResponse<string> = {
            item: "",
            message: "Failed to find token",
            success: false
        };
        return failedTokenResponse
    }

    const newUser: User = {
        email: email,
        password: password,
        companyName: companyName,
        role: role,
        createdDate: date
    };

    const response: DataResponse<User> = await axios({
        url: url + "register",
        method: "post",
        headers: {
            'Authorization': `Bearer: ${token}`,
            'Content-Type': 'application/json'
        },
        data: newUser
    });

    if (response.item && response.success == true) {
        return response.item;
    } else {
        const failedCreateResponse: DataResponse<string> = {
            item: "",
            message: "Failed to create user",
            success: false
        };
        return failedCreateResponse;
    }
}