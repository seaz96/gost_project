import useGostsWithPagination from "hooks/useGostsWithPagination.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Filter from "../../../components/Filter/Filter.tsx";
import FilterTabs from "../../../components/FilterTabs/FilterTabs.tsx";
import GostsTable from "../../../components/GostsTable/GostsTable.tsx";
import type {status} from "../../../entities/gost/gostModel.ts";
import styles from "./GostsPage.module.scss";

const GostsPage = () => {
	const { gosts, countFetched, count, setGostParams, gostsParams, fetchGostsData } =
		useGostsWithPagination("/docs/search");

	return (
		<main className="container">
			<h1>Документы</h1>
			<section className="verticalPadding">
				<Filter filterSubmit={setGostParams}/>
			</section>
			<section className="verticalPadding">
				<FilterTabs
					tabs={[
						{title: "Все", value: "All"},
						{title: "Действующие", value: "Valid"},
						{title: "Отменённые", value: "Canceled"},
						{title: "Заменённые", value: "Replaced"},
					]}
					activeTabs={[gostsParams.SearchFilters?.Status ?? "All"]}
					setActiveTabs={(activeTabs) =>
						setGostParams({...gostsParams,
							SearchFilters: {
								...gostsParams.SearchFilters,
								Status: activeTabs[0] !== "All" ? activeTabs[0] as status : null
							}
						})
					}
				/>
			</section>
			<div className="verticalPadding">Найдено {count} документов</div>
			<div>
				<section className={styles.gostSection}>
					<InfiniteScroll
						dataLength={countFetched}
						next={fetchGostsData}
						hasMore={count > countFetched}
						loader={<h4>Загрузка</h4>}
						endMessage={<p>Конец таблицы</p>}
					>
						<GostsTable gosts={gosts} gostsParams={gostsParams}/>
					</InfiniteScroll>
				</section>
			</div>
		</main>
	);
};

export default GostsPage;