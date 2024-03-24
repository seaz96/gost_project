import React, { useEffect, useState } from 'react'
import { axiosInstance } from 'shared/configs/axiosConfig';
import { Gost } from '../model/gostModel';

const baseLimit = 1

export const useGostsWithPagination = (url: string, defaultParams?:any) => {
    const [gosts, setGosts] = useState<Gost[]>([]);
    const [activeGosts, setActiveGosts] = useState<Gost[]>([])
    const [error, setError] = useState<string>('');
    const [loading, setloading] = useState<boolean>(true);
    const [gostsParams, setGostParams] = useState<any>(defaultParams)
    const [lastId, setLastId] = useState(0)
    const [count, setCount] = useState(0)
    const [page, setPage] = useState(1)

    const fetchGostsData = (limit: number, id: number = lastId, refetch: boolean = false) => {
        return axiosInstance
            .get(url, {params: {...gostsParams, lastId: id, limit: limit}})
            .then((res) => {
                const data = res.data as Gost[]
                console.log(data)
                setActiveGosts(data.slice(res.data.length - baseLimit));
                if(refetch) {
                    setGosts(data);
                } else {
                    setGosts((prevGosts) => [...prevGosts, ...data]);
                }
                setLastId(data[data.length - 1].docId);
            })
            .catch((err: any) => {
                setError(err);
            })
    };

    const fetchCountData = () => {
        return axiosInstance
            .get(url + '-count', {params: {...gostsParams}})
            .then((res) => {
                setCount(Math.ceil(res.data / baseLimit));
            })
            .catch((err: any) => {
                setError(err);
            })
    };

    const changePage = (page: number) => {
        setPage(page)
        if(page * baseLimit > gosts.length) {
            fetchGostsData(page * baseLimit - gosts.length)
        } else {
            setActiveGosts(gosts.slice((page-1)*baseLimit, page*baseLimit))
        }
    }

    useEffect(() => {
        setloading(true)
        setLastId(0)
        fetchCountData()
        .then(() => fetchGostsData(baseLimit, 0, true)
        .then(() => setloading(false)));
    }, [gostsParams, url]);


    return { activeGosts, count, page, error, loading, setGostParams, changePage };
}
