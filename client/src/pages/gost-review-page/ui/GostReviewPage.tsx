import { Link, useParams } from "react-router-dom";
import GostReview from "../../../components/GostReview/GostReview.tsx";
import { useFetchGostQuery } from "../../../features/api/apiSlice";

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
			<main className="container">
				<GostReview gost={gost} />
			</main>
		);
	}
	return <></>;
};

export default GostReviewPage;
