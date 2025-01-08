import classNames from "classnames";
import { useNavigate, useParams } from "react-router-dom";
import GostForm from "../../../components/GostForm/GostForm.tsx";
import type { GostRequestModel } from "../../../entities/gost/gostModel.ts";
import { useFetchGostQuery, useUpdateGostMutation, useUploadGostFileMutation } from "../../../features/api/apiSlice";
import styles from "./GostEditPage.module.scss";

const GostEditPage = () => {
	const navigate = useNavigate();
	const id = useParams().id;
	const { data: gost } = useFetchGostQuery(id!);
	const [updateGost] = useUpdateGostMutation();
	const [uploadFile] = useUploadGostFileMutation();

	const editOldDocument = async (gostData: GostRequestModel, file: File) => {
		await updateGost({ id: id!, gost: gostData });
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
					<GostForm
						handleUploadFile={handleUploadFile}
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
