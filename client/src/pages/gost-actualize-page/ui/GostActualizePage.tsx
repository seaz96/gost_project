import classNames from "classnames";
import { useNavigate, useParams } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type { GostToSave } from "../../../components/GostForm/newGostModel.ts";
import { useActualizeGostMutation, useFetchGostQuery, useUploadGostFileMutation } from "../../../features/api/apiSlice";
import styles from "./GostActualizePage.module.scss";

const GostActualizePage = () => {
	const id = useParams().id;
	const { data: gost } = useFetchGostQuery(id!);
	const [actualizeGost] = useActualizeGostMutation();
	const [uploadFile] = useUploadGostFileMutation();
	const navigate = useNavigate();

	const addNewDocument = async (gostData: GostToSave, file: File) => {
		await actualizeGost({ id: id!, gost: gostData });
		await handleUploadFile(file, id);
		navigate(`/gost-review/${id}`);
	};

	const handleUploadFile = async (file: File, docId: string | undefined) => {
		if (docId) {
			await uploadFile({ docId, file });
		}
	};

	if (gost)
		return (
			<div className="container">
				<section className={classNames("contentContainer", styles.reviewSection)}>
					<h2>Актуализировать данные</h2>
					<GostForm
						handleUploadFile={handleUploadFile}
						handleSubmit={addNewDocument}
						gost={{
							...gost.primary,
							references: gost.references.map((reference) => reference.designation),
						}}
					/>
				</section>
			</div>
		);
	return <></>;
};

export default GostActualizePage;
