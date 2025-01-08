import classNames from "classnames";
import { useNavigate, useParams } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type {GostAddModel} from "../../../entities/gost/gostModel.ts";
import { useFetchGostQuery, useUpdateGostMutation } from "../../../features/api/apiSlice";
import styles from "./GostEditPage.module.scss";

const GostEditPage = () => {
	const navigate = useNavigate();
	const id = useParams().id;

	if (!id) {
		navigate("/");
		return null;
	}

	const { data: gost } = useFetchGostQuery(id);
	const [updateGost] = useUpdateGostMutation();

	const editOldDocument = async (gostData: GostAddModel) => {
		await updateGost({ id: id, gost: gostData });
		navigate(`/gost-review/${id}`);
	};

	if (gost)
		return (
			<div className="container">
				<section className={classNames("contentContainer", styles.reviewSection)}>
					<GostForm
						handleSubmit={editOldDocument}
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

export default GostEditPage;