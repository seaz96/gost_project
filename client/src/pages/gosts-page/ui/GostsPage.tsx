import useGostsWithPagination from "hooks/useGostsWithPagination.ts";
import InfiniteScroll from "react-infinite-scroll-component";
import Filter from "../../../components/Filter/Filter.tsx";
import GostsTable from "../../../components/GostsTable/GostsTable.tsx";
import styles from "./GostsPage.module.scss";

const GostsPage = () => {
	const { gosts, countFetched, count, setGostParams, gostsParams, fetchGostsData } =
		useGostsWithPagination("/docs/search");

	return (
		<div className="container contentContainer">
			<section className={styles.filterSection}>
				<Filter filterSubmit={setGostParams} />
			</section>
			<div>
				<section className={styles.gostSection}>
					<InfiniteScroll
						dataLength={countFetched}
						next={fetchGostsData}
						hasMore={count > countFetched}
						loader={<h4>Загрузка</h4>}
						endMessage={<p>Конец таблицы</p>}
					>
						<GostsTable gosts={gosts} gostsParams={gostsParams} />
					</InfiniteScroll>
				</section>
			</div>
		</div>
	);
};

export default GostsPage;