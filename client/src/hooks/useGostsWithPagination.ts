import type {GostSearchParams, GostViewInfo} from "entities/gost/gostModel";
import {useFetchGostsCountQuery, useLazyFetchGostsPageQuery} from "features/api/apiSlice";
import {useEffect, useState} from "react";

const PAGE_SIZE = 10;
const SMART_SEARCH_URL = "/docs/search";
const SEARCH_URL = "/docs/all";

const useGostsWithPagination = (useSmartSearch: boolean) => {
	const [params, setParams] = useState<GostSearchParams & { text?: string }>(
		{} as GostSearchParams & { text?: string },
	);
	const [offset, setOffset] = useState(0);
	const [accumulatedGosts, setAccumulatedGosts] = useState<GostViewInfo[]>([]);
	const [fetching, setFetching] = useState(false);

	const [trigger] = useLazyFetchGostsPageQuery();

	const { data: totalCount = 0 } = useFetchGostsCountQuery({
		url: useSmartSearch ? SMART_SEARCH_URL : SEARCH_URL,
		params: params,
	});

	// biome-ignore lint/correctness/useExhaustiveDependencies: when useSmartSearch changes, reset offset
	useEffect(() => {
		handleFilterSubmit(params);
	}, [useSmartSearch]);

	const loadMore = () => {
		setFetching(true)
	};

	useEffect(() => {
		if (!fetching) return;
		console.log(`Fetching data with offset ${offset}`);
		trigger({
			url: useSmartSearch ? SMART_SEARCH_URL : SEARCH_URL,
			offset: offset,
			limit: PAGE_SIZE,
			params: params,
		}).then((res) => {
			if (res.data && Array.isArray(res.data)) {
				if (res.data.length === 0) {
					console.log(
						`No data fetched. Current page is ${offset / PAGE_SIZE}, total: ${totalCount}, total fetched: ${accumulatedGosts.length}`,
					);
					return;
				}
				console.log(`Fetched ${res.data.length} documents. Total ${accumulatedGosts.length + res.data.length}`);
				setAccumulatedGosts((prev) => [...prev, ...(res.data as [])]);
			}
		});
		setOffset((prev) => prev + PAGE_SIZE);
		setFetching(false);
	}, [fetching, params, useSmartSearch, trigger, offset, accumulatedGosts, totalCount]);

	const handleFilterSubmit = (filterData: GostSearchParams & { text?: string }) => {
		console.log("Resetting due to filter change");
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