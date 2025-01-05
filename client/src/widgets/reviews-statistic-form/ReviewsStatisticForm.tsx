import type React from "react";
import { useState } from "react";
import { Button, Input } from "../../shared/components";

import { axiosInstance } from "../../shared/configs/axiosConfig.ts";
import styles from "./ReviewsStatisticForm.module.scss";

interface ReviewsStatisticFormProps {
	handleSubmit: Function;
	startDateSubmit: Function;
	endDateSubmit: Function;
}

const ReviewsStatisticForm: React.FC<ReviewsStatisticFormProps> = (props) => {
	const { handleSubmit, startDateSubmit, endDateSubmit } = props;

	const [reviewsData, setReviewsData] = useState({
		designation: "",
		codeOks: "",
		activityField: "",
		startDate: "",
		endDate: "",
	});

	const validateData = (event: React.FormEvent) => {
		event.preventDefault();

		if (reviewsData.startDate && new Date(reviewsData.startDate).toString() !== "Invalid Date") {
			reviewsData.startDate = new Date(reviewsData.startDate).toISOString();
		} else {
			reviewsData.startDate = new Date("1970-01-01").toISOString();
		}

		if (reviewsData.endDate && new Date(reviewsData.endDate).toString() !== "Invalid Date") {
			reviewsData.endDate = new Date(reviewsData.endDate).toISOString();
		} else {
			reviewsData.endDate = new Date().toISOString();
		}

		axiosInstance
			.get("/stats/get-views", {
				params: {
					...reviewsData,
					StartDate: reviewsData.startDate,
					EndDate: reviewsData.endDate,
				},
			})
			.then((response) => {
				handleSubmit(response.data);
				startDateSubmit(reviewsData.startDate);
				endDateSubmit(reviewsData.endDate);
			});
	};

	return (
		<form className={styles.form} onSubmit={(event) => validateData(event)}>
			<Input
				label="Название ГОСТа"
				type="text"
				value={reviewsData.designation}
				onChange={(value: string) => setReviewsData({ ...reviewsData, designation: value })}
			/>
			<Input
				label="Код ОКС"
				type="text"
				value={reviewsData.codeOks}
				onChange={(value: string) => setReviewsData({ ...reviewsData, codeOks: value })}
			/>
			<Input
				label="Сфера деятельности"
				type="text"
				value={reviewsData.activityField}
				onChange={(value: string) => setReviewsData({ ...reviewsData, activityField: value })}
			/>
			<p className={styles.datesTitle}>Даты обращение к карте(от, до)</p>
			<Input
				placeholder="От..."
				type="date"
				value={reviewsData.startDate}
				onChange={(value: string) => setReviewsData({ ...reviewsData, startDate: value })}
			/>
			<Input
				placeholder="До..."
				type="date"
				value={reviewsData.endDate}
				onChange={(value: string) => setReviewsData({ ...reviewsData, endDate: value })}
			/>
			<Button onClick={() => {}} className={styles.formButton} isFilled type="submit">
				Сформировать отчёт
			</Button>
		</form>
	);
};

export default ReviewsStatisticForm;