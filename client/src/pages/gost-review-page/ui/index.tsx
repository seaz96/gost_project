import GostReview from "../../../widgets/gost-review/GostReview.tsx";

import classNames from "classnames";
import type {gostModel} from "entities/gost";
import {useParams} from "react-router-dom";
import {useAxios} from "shared/hooks";
import styles from "./GostReviewsPage.module.scss";

const GostReviewPage = () => {
	const gostId = useParams().id;
	const { response, error, loading } = useAxios<gostModel.Gost>(`/docs/${gostId}`);

	if (response) {
		return (
			<div className="container">
				<section className={classNames(styles.reviewSection, "contentContainer")}>
					<GostReview gost={response} gostId={response.docId} />
				</section>
			</div>
		);
	}
	return <></>;
};

export default GostReviewPage;