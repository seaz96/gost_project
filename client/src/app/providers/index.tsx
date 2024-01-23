import compose from "compose-function";
import { withRouter } from "./with-router";
import { withUser } from "./with-user";

export const withProviders = compose(withRouter, withUser);