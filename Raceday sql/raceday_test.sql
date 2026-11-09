select r.result_id,u.first_name+' '+u.last_name as participant,e.event_name,r.finish_time,r.finishing_position,r.result_status
from result r
join enrolment en on r.enrolment_id=en.enrolment_id
join [user] u on en.participant_id=u.user_id
join eventcategory ec on en.event_category_id=ec.event_category_id
join event e on ec.event_id=e.event_id;