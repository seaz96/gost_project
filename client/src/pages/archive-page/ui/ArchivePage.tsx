import type { gostModel } from "entities/gost";
import InfiniteScroll from "react-infinite-scroll-component";
import Filter from "../../../components/Filter/Filter.tsx";
import GostsTable from "../../../components/GostsTable/GostsTable.tsx";
import useGostsWithPagination from "../../../hooks/useGostsWithPagination.ts";
import styles from "./ArchivePage.module.scss";
const ArchivePage = () => {
	const { gosts, countFetched, count, setGostParams, gostsParams, fetchGostsData } =
		useGostsWithPagination("/docs/all-canceled");

	return (
		<div className="container contentContainer">
			<InfiniteScroll
				dataLength={countFetched}
				next={fetchGostsData}
				hasMore={count > countFetched}
				loader={
					<p style={{ textAlign: "center" }}>
						<b>Загрузка...</b>
					</p>
				}
			>
				<section className={styles.filterSection}>
					<Filter filterSubmit={(filterData: gostModel.GostFields & { text?: string }) => setGostParams(filterData)} />
				</section>
				<section className={styles.gostSection}>
					<GostsTable gosts={gosts || []} gostsParams={gostsParams} />
				</section>
				<section>
					{count === 0 ? (
						<p style={{ textAlign: "center" }}>
							<b>Документов нет.</b>
						</p>
					) : null}
				</section>
			</InfiniteScroll>
		</div>
	);
};

export default ArchivePage;
