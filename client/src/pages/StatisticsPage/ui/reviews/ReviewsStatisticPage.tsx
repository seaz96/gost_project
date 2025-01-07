import {useEffect, useRef, useState} from "react";
import ReviewsStatisticForm from "../../../../components/ReviewsStatisticForm/ReviewsStatisticForm.tsx";
import ReviewsStatisticTable from "../../../../components/ReviewsStatisticTable/ReviewsStatisticTable.tsx";
import type {GostViews} from "../../../../entities/gost/gostModel.ts";

const ReviewsStatisticPage = () => {
	const [reviewsData, setReviewsData] = useState<GostViews[] | null>(null);
	const [startDate, setStartDate] = useState("");
	const [endDate, setEndDate] = useState("");
	const reportRef = useRef<HTMLElement>(null);

	const handleSubmit = (values: GostViews[]) => {
		setReviewsData(values);
	};

	useEffect(() => {
		if (reportRef.current && reviewsData) {
			reportRef.current.scrollIntoView({ behavior: "smooth" });
		}
	}, [reviewsData]);

	return (
		<section>
			<section>
				<ReviewsStatisticForm
					handleSubmit={handleSubmit}
					startDateSubmit={setStartDate}
					endDateSubmit={setEndDate}
				/>
			</section>
			{reviewsData && (
				<section className={"verticalPadding"} ref={reportRef}>
					<h2>Отчёт об обращениях</h2>
					<p className="verticalPadding">
						{`с ${formatDate(new Date(startDate))} по ${formatDate(new Date(endDate))}`}
					</p>
					<ReviewsStatisticTable reviewsData={reviewsData} />
				</section>
			)}
		</section>
	);
};

const formatDate = (date: Date) => {
	const day = date.getDate();
	const month = date.getMonth() + 1;
	const year = date.getFullYear();
	return `${day}.${month}.${year}`;
};

export default ReviewsStatisticPage;