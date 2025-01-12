import classNames from "classnames";
import { Link, useNavigate, useParams, useSearchParams } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type { GostAddModel } from "../../../entities/gost/gostModel.ts";
import { useFetchGostQuery, useUpdateGostMutation } from "../../../features/api/apiSlice";
import styles from "./GostEditPage.module.scss";

const GostEditPage = () => {
	const navigate = useNavigate();
	const id = useParams().id;
	const [searchParams] = useSearchParams();
	const isActualize = searchParams.has("actualize");

	if (!id) {
		navigate("/");
		return null;
	}

	const { data: gost } = useFetchGostQuery(id);
	const [updateGost] = useUpdateGostMutation();

	const handleSubmit = async (gostData: GostAddModel) => {
		await updateGost({ id: id, gost: gostData, actualize: isActualize }).then(() => navigate(`/gost-review/${id}`));
	};

	if (gost)
		return (
			<div className="container">
				<h1 className={"verticalPadding"}>{isActualize ? "Актуализировать" : "Редактировать"} документ</h1>
				<Link className={styles.back} to={`/gost-review/${id}`}>
					Вернуться к просмотру
				</Link>
				<section className={classNames("contentContainer", styles.reviewSection)}>
					<GostForm
						handleSubmit={handleSubmit}
						data={{
							...gost.primary,
							status: gost.status,
							references: gost.references.map((reference) => reference.designation),
						}}
					/>
				</section>
			</div>
		);
	return <></>;
};

export default GostEditPage;