import * as React from 'react';

import { RouteComponentProps } from 'react-router';

import { Link, NavLink } from 'react-router-dom';

interface FetchEmployeeDataState {
    empList: EmployeeData[];
    loading: boolean;
}

export class FetchEmployee extends React.Component<RouteComponentProps<{}>, FetchEmployeeDataState>{
    constructor()


}