import classNames from "classnames";
import { useNavigate } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type {GostAddModel} from "../../../entities/gost/gostModel.ts";
import { useAddGostMutation } from "../../../features/api/apiSlice";
import styles from "./GostCreatorPage.module.scss";

const GostCreatorPage = () => {
	const navigate = useNavigate();
	const [addGost] = useAddGostMutation();

	const addNewDocument = async (gost: GostAddModel) => {
		const response = await addGost(gost).unwrap();
		navigate(`/gost-review/${response}`);
	};

	return (
		<div className="container">
			<section className={classNames("contentContainer", styles.reviewSection)}>
				<GostForm handleSubmit={addNewDocument} />
			</section>
		</div>
	);
};

export default GostCreatorPage;