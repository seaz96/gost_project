import classNames from "classnames";
import { useNavigate, useParams } from "react-router-dom";
import GostForm, {getGostStub} from "../../../components/GostForm/GostForm.tsx";
import type {GostToSave} from "../../../components/GostForm/newGostModel.ts";
import { 
    useAddGostMutation, 
    useChangeGostStatusMutation, 
    useFetchGostQuery, 
    useUploadGostFileMutation 
} from "../../../features/api/apiSlice";
import styles from "./GostReplacePage.module.scss";

const GostReplacePage = () => {
    const navigate = useNavigate();
    const gostToReplaceId = useParams().id;
    const { data: gost } = useFetchGostQuery(gostToReplaceId!);
    const [addGost] = useAddGostMutation();
    const [changeStatus] = useChangeGostStatusMutation();
    const [uploadFile] = useUploadGostFileMutation();

    const addNewDocument = async (gost: GostToSave, file: File) => {
        await addGost(gost);
        await changeStatus({ id: gostToReplaceId!, status: 2 });
        await handleUploadFile(file, gostToReplaceId);
        navigate("/");
    };

    const handleUploadFile = async (file: File, docId: string | undefined) => {
        if (docId) {
            await uploadFile({ docId, file });
        }
    };

    if (!gost) return <></>;

    return (
        <div className="container">
            <section className={classNames("contentContainer", styles.reviewSection)}>
                <GostForm
                    handleUploadFile={handleUploadFile}
                    handleSubmit={addNewDocument}
                    gost={{
                        //FIXME
                        ...getGostStub(),
                        acceptedFirstTimeOrReplaced: `Принят взамен ${gost?.primary.designation}`,
                    }}
                />
            </section>
        </div>
    );
};

export default GostReplacePage;