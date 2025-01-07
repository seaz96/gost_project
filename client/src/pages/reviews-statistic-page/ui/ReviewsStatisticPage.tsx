import { useState } from "react";

import classNames from "classnames";
import type { gostModel } from "entities/gost";
import ReviewsStatisticForm from "../../../components/ReviewsStatisticForm/ReviewsStatisticForm.tsx";
import ReviewsStatisticTable from "../../../components/ReviewsStatisticTable/ReviewsStatisticTable.tsx";
import styles from "./ReviewsStatisticPage.module.scss";

const ReviewsStatisticPage = () => {
	const [reviewsData, setReviewsData] = useState<gostModel.GostViews[] | null>(null);
	const [startDate, setStartDate] = useState("");
	const [endDate, setEndDate] = useState("");

	return (
		<div className="container">
			{reviewsData ? (
				<section className={classNames(styles.statistic, "contentContainer")}>
					<h2 className={styles.title}>
						<span>Статистика</span> с {`${formatDate(new Date(startDate))}`} по {`${formatDate(new Date(endDate))}`}
					</h2>
					<ReviewsStatisticTable reviewsData={reviewsData} />
				</section>
			) : (
				<section className={classNames(styles.statistic, "contentContainer")}>
					<ReviewsStatisticForm
						handleSubmit={(values: gostModel.GostViews[]) => setReviewsData(values)}
						startDateSubmit={setStartDate}
						endDateSubmit={setEndDate}
					/>
				</section>
			)}
		</div>
	);
};

const formatDate = (date: Date) => {
	const day = date.getDate();
	const month = date.getMonth() + 1;
	const year = date.getFullYear();
	return `${day}.${month}.${year}`;
};

export default ReviewsStatisticPage;
