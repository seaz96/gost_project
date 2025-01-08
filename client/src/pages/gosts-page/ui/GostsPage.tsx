import useGostsWithPagination from "hooks/useGostsWithPagination.ts";
import {useEffect, useRef} from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import {Link} from "react-router-dom";
import {useAppSelector} from "../../../app/hooks.ts";
import Filter from "../../../components/Filter/Filter.tsx";
import FilterButton from "../../../components/FilterButton/FilterButton.tsx";
import FilterTabs from "../../../components/FilterTabs/FilterTabs.tsx";
import GostsTable from "../../../components/GostsTable/GostsTable.tsx";
import {
	AdoptionLevelToRu,
	HarmonizationToRu,type adoptionLevel, 
	type harmonization,
	type status 
} from "../../../entities/gost/gostModel.ts";
import styles from "./GostsPage.module.scss";

const GostsPage = () => {
	const { gosts, countFetched, count, setGostParams, gostsParams, fetchGostsData } =
		useGostsWithPagination("/docs/search");
	const user = useAppSelector((s) => s.user.user);
	const contentRef = useRef<HTMLDivElement>(null);

	// biome-ignore lint/correctness/useExhaustiveDependencies: hook
	useEffect(() => {
		if (contentRef.current && contentRef.current.clientHeight < window.innerHeight) {
			fetchGostsData();
		}
	}, [countFetched]);

	return (
		<main className="container">
			<h1 className="verticalPadding">Документы</h1>
			{(user?.role === "Admin" || user?.role === "Heisenberg") && (
				<Link to="/gost-editor" className="verticalPadding">
					Создать документ
				</Link>
			)}
			<section className="verticalPadding">
				<Filter filterSubmit={setGostParams} />
			</section>
			<section className="verticalPadding">
				<FilterTabs
					tabs={[
						{ title: "Все", value: "All" },
						{ title: "Действующие", value: "Valid" },
						{ title: "Отменённые", value: "Canceled" },
						{ title: "Заменённые", value: "Replaced" },
					]}
					activeTabs={[gostsParams.SearchFilters?.Status ?? "All"]}
					setActiveTabs={(activeTabs) =>
						setGostParams({
							...gostsParams,
							SearchFilters: {
								...gostsParams.SearchFilters,
								Status: activeTabs[0] !== "All" ? (activeTabs[0] as status) : null,
							},
						})
					}
				/>
			</section>
			<section className="verticalPadding flex">
				<FilterButton
					title="Уровень гармонизации"
					options={Object.entries(HarmonizationToRu).map(([value, label]) => ({ value, label }))}
					selectedOptions={gostsParams.SearchFilters?.Harmonization ? [gostsParams.SearchFilters.Harmonization] : []}
					setSelectedOptions={(options) => {
						setGostParams({
							...gostsParams,
							SearchFilters: {
								...gostsParams.SearchFilters,
								Harmonization: (options[0] as harmonization) ?? null,
							},
						});
					}}
				/>
				<FilterButton
					title="Уровень принятия"
					options={Object.entries(AdoptionLevelToRu).map(([value, label]) => ({ value, label }))}
					selectedOptions={gostsParams.SearchFilters?.AdoptionLevel ? [gostsParams.SearchFilters.AdoptionLevel] : []}
					setSelectedOptions={(options) => {
						setGostParams({
							...gostsParams,
							SearchFilters: {
								...gostsParams.SearchFilters,
								AdoptionLevel: (options[0] as adoptionLevel) ?? null,
							},
						});
					}} />
			</section>
			<div className="verticalPadding">Найдено {count} документов</div>
			<div ref={contentRef}>
				<section className="verticalPadding">
					<InfiniteScroll
						dataLength={countFetched}
						next={fetchGostsData}
						hasMore={count > countFetched}
						loader={<TableLoader />}
						endMessage={<TableEnd />}
					>
						<GostsTable gosts={gosts} gostsParams={gostsParams} />
					</InfiniteScroll>
				</section>
			</div>
		</main>
	);
};

const TableEnd = () => {
	return <div className={styles.tableEnd}>Конец таблицы</div>;
};

const TableLoader = () => {
	return <div className={styles.tableLoad}>Загрузка</div>;
};

export default GostsPage;