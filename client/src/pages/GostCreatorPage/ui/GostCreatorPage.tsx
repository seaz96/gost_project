import classNames from "classnames";
import { useNavigate } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type { GostToSave } from "../../../components/GostForm/newGostModel.ts";
import { useAddGostMutation, useUploadGostFileMutation } from "../../../features/api/apiSlice";
import styles from "./GostCreatorPage.module.scss";

const GostCreatorPage = () => {
	const navigate = useNavigate();
	const [addGost] = useAddGostMutation();
	const [uploadFile] = useUploadGostFileMutation();

	const addNewDocument = async (gost: GostToSave, file: File) => {
		const response = await addGost(gost).unwrap();
		await handleUploadFile(file, response);
		navigate(`/gost-review/${response}`);
	};

	const handleUploadFile = async (file: File, docId: string | undefined) => {
		if (docId) {
			await uploadFile({ docId, file });
		}
	};

	return (
		<div className="container">
			<section className={classNames("contentContainer", styles.reviewSection)}>
				<GostForm handleUploadFile={handleUploadFile} handleSubmit={addNewDocument} />
			</section>
		</div>
	);
};

export default GostCreatorPage;