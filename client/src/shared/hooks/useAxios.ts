import axios from "axios";
import { useEffect, useState } from "react";

const useAxios = <T>(url: string, defaultParams?:any) => {
    const [response, setResponse] = useState<T | null>(null);
    const [error, setError] = useState<string>('');
    const [loading, setloading] = useState<boolean>(true);
    const [params, setParams] = useState<any>(defaultParams)

    const fetchData = () => {
        console.log(params)
        axios
            .get(url, {headers: {Authorization: `Bearer ${localStorage.getItem('jwt_token')}`}, params: {...params}})
            .then((res) => {
                setResponse(res.data || null);
                console.log(res)
            })
            .catch((err: any) => {
                setError(err);
                console.log(err)
            })
            .finally(() => {
                setloading(false);
            });
    };

    useEffect(() => {
        fetchData();
    }, [params]);


    return { response, error, loading, setParams };
};

export default useAxios;