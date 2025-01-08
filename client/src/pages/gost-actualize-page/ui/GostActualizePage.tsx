import classNames from "classnames";
import { useNavigate, useParams } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type { GostRequestModel } from "../../../entities/gost/gostModel.ts";
import { useActualizeGostMutation, useFetchGostQuery } from "../../../features/api/apiSlice";
import styles from "./GostActualizePage.module.scss";

const GostActualizePage = () => {
	const id = useParams().id;
	const { data: gost } = useFetchGostQuery(id!);
	const [actualizeGost] = useActualizeGostMutation();
	const navigate = useNavigate();

	const addNewDocument = async (gostData: GostRequestModel) => {
		await actualizeGost({ id: id!, gost: gostData });
		navigate(`/gost-review/${id}`);
	};

	if (gost)
		return (
			<div className="container">
				<section className={classNames("contentContainer", styles.reviewSection)}>
					<h2>Актуализировать данные</h2>
					<GostForm
						handleSubmit={addNewDocument}
						gost={{
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

export default GostActualizePage;