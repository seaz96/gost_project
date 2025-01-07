import type { GostSearchParams, GostViewInfo } from "entities/gost/gostModel";
import { useFetchGostsCountQuery, useFetchGostsPageQuery } from "features/api/apiSlice";
import { useEffect, useState } from "react";

const PAGE_SIZE = 10;

const useGostsWithPagination = (url: string) => {
	const [params, setParams] = useState<GostSearchParams & { text?: string }>(
		{} as GostSearchParams & { text?: string },
	);
	const [offset, setOffset] = useState(0);
	const [accumulatedGosts, setAccumulatedGosts] = useState<GostViewInfo[]>([]);

	const { data: currentPageGosts = [] } = useFetchGostsPageQuery({
		url,
		offset,
		limit: PAGE_SIZE,
		params,
	});

	const { data: totalCount = 0 } = useFetchGostsCountQuery({
		url,
		params,
	});

	useEffect(() => {
		if (!currentPageGosts.length) return;
		if (offset === 0) {
			setAccumulatedGosts(currentPageGosts);
		} else {
			setAccumulatedGosts((prev) => [
				...prev,
				...currentPageGosts.filter((item) => !prev.some((p) => p.id === item.id)),
			]);
		}
	}, [currentPageGosts, offset]);

	const loadMore = () => {
		setOffset((prev) => prev + PAGE_SIZE);
	};

	const handleFilterSubmit = (filterData: GostSearchParams & { text?: string }) => {
		setOffset(0);
		setAccumulatedGosts([]);
		setParams(filterData);
	};

	return {
		gosts: accumulatedGosts,
		countFetched: accumulatedGosts.length,
		count: totalCount,
		setGostParams: handleFilterSubmit,
		gostsParams: params,
		fetchGostsData: loadMore,
	};
};

export default useGostsWithPagination;
