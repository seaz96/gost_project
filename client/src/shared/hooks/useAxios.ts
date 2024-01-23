import axios from "axios";
import { useEffect, useState } from "react";

const useAxios = <T>(url: string) => {
    const [response, setResponse] = useState<T | null>(null);
    const [error, setError] = useState<string>('');
    const [loading, setloading] = useState<boolean>(true);

    const fetchData = () => {
        axios
            .get(url, {headers: {Authorization: `Bearer ${localStorage.getItem('jwt_token')}`}})
            .then((res) => {
                setResponse(res.data || null);
            })
            .catch((err) => {
                setError(err);
            })
            .finally(() => {
                setloading(false);
            });
    };

    useEffect(() => {
        fetchData();
    }, []);


    return { response, error, loading };
};

export default useAxios;