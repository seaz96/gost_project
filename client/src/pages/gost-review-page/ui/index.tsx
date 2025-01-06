import classNames from "classnames";
import { Link, useParams } from "react-router-dom";
import GostReview from "../../../components/GostReview/GostReview.tsx";
import { useFetchGostQuery } from "../../../features/api/apiSlice";
import styles from "./GostReviewsPage.module.scss";

const GostReviewPage = () => {
	const gostId = useParams().id;

	if (!gostId)
		return (
			<>
				ID не указан. Вернитесь на <Link to={"/"}>главную</Link>
			</>
		);

	const { data: gost } = useFetchGostQuery(gostId);

	if (gost) {
		return (
			<div className="container">
				<section className={classNames(styles.reviewSection, "contentContainer")}>
					<GostReview gost={gost} gostId={gost.docId} />
				</section>
			</div>
		);
	}
	return <></>;
};

export default GostReviewPage;
