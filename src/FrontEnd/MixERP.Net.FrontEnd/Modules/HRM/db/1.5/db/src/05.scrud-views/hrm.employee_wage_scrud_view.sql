DROP VIEW IF EXISTS hrm.employee_wage_scrud_view;

CREATE VIEW hrm.employee_wage_scrud_view
AS
SELECT
    hrm.employee_wages.employee_wage_id,
    hrm.employees.employee_code || ' (' || hrm.employees.employee_name || ')' AS employee,
    hrm.employees.photo,
    hrm.wage_setup.wage_setup_code || ' (' || hrm.wage_setup.wage_setup_name || ')' AS wage_setup,
    hrm.employee_wages.currency_code,
    hrm.employee_wages.max_week_hours,
    hrm.employee_wages.hourly_rate,
    hrm.employee_wages.overtime_applicable,
    hrm.employee_wages.overtime_hourly_rate,
    hrm.employee_wages.valid_till,
    hrm.employee_wages.is_active
FROM hrm.employee_wages
INNER JOIN hrm.employees
ON hrm.employee_wages.employee_id = hrm.employees.employee_id
INNER JOIN hrm.wage_setup
ON hrm.wage_setup.wage_setup_id = hrm.employee_wages.wage_setup_id;