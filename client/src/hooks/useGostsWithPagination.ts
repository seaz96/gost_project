import { useEffect, useState } from "react";
import type { GostViewInfo } from "../entities/gost/gostModel.ts";
import { axiosInstance } from "../shared/configs/axiosConfig.ts";

const baseLimit = 10;

export const useGostsWithPagination = (url: string, defaultParams?: any) => {
	const [gosts, setGosts] = useState<GostViewInfo[]>([]);
	const [error, setError] = useState<string>("");
	const [loading, setloading] = useState<boolean>(true);
	const [gostsParams, setGostParams] = useState<any>(defaultParams);
	const [lastId, setLastId] = useState(0);
	const [count, setCount] = useState(0);
	const [countFetched, setCountFetched] = useState(0);

	const fetchGostsData = async (limit: number = baseLimit, id: number = lastId, refetch = false) => {
		return axiosInstance
			.get(url, { params: { ...gostsParams, lastId: id, limit: limit } })
			.then((res) => {
				const data = res.data as GostViewInfo[];
				if (refetch) {
					setGosts(data);
				} else {
					setGosts((prevGosts) => [...prevGosts, ...data]);
				}
				setLastId(data[data.length - 1].id);
				setCountFetched((prev) => prev + limit);
			})
			.catch((err: any) => {
				console.error(err);
			});
	};

	const fetchCountData = () => {
		return axiosInstance
			.get(url + "-count", { params: { ...gostsParams } })
			.then((res) => {
				setCount(res.data);
			})
			.catch((err: any) => {
				console.error(err);
			});
	};

	useEffect(() => {
		setGosts([]);
		setloading(true);
		setLastId(0);
		setCountFetched(0);
		fetchCountData().then(() => fetchGostsData(baseLimit, 0, true).then(() => setloading(false)));
	}, [gostsParams, url]);

	return {
		gosts,
		count,
		countFetched,
		error,
		loading,
		gostsParams,
		setGostParams,
		fetchGostsData,
	};
};